using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TwitterStreamingLib.Abstraction;
using TwitterStreamingLib.Configuration;
using TwitterStreamingLib.Core;
using TwitterStreamingLib.DataStructures;

namespace TwitterStreamingConsole;

public class Program
{
    private static void BuildConfig(IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }

    
    private static void RegisterCustomServices(IServiceCollection services, IConfiguration configuration)
    {
        var listenerConfiguration = new ListenerConfiguration(configuration);
        services.AddSingleton<ListenerConfiguration>(listenerConfiguration);

        // This section needs to be a singleton because the in-memory management of the data
        var hashtagAppearances = new HashtagAppearances();

        Log.Logger.Information("Hashcode for hastagAppearances {GetHashCode}", hashtagAppearances.GetHashCode());
        services.AddSingleton<HashtagAppearances>(hashtagAppearances);

        // Need to instantiate so we can refer to the same object but with different behaviors
        var agreggatedService = new TweetRepository(hashtagAppearances);
        services.AddSingleton<ITweetRepository>(agreggatedService);
        services.AddSingleton<ITwitterAnalysis>(agreggatedService);

        // Instantiate and configure HttpClient
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new("Bearer", listenerConfiguration.BearerToken);
        services.AddSingleton<HttpClient>(httpClient);

        // Regular transient - instantiate, dispose as many times as necessary. In this case only once.
        services.AddTransient<ITweetHandler, TweetHandler>();
        services.AddTransient<ITwitterStreamListener, TwitterStreamListener>();
        services.AddTransient<ConsoleReporting>();
    }

    private static IHost Startup()
    {
        var builder = new ConfigurationBuilder();
        BuildConfig(builder);

        var configuration = builder.Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        Log.Logger.Information("Starting Application...");

        return Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                RegisterCustomServices(services, configuration);
            })
            .UseSerilog()
            .Build();
    }

    
    static async Task Main(string[] args)
    {
        var host = Startup();

        var listener = host.Services.GetService<ITwitterStreamListener>();
        
        var reporting = host.Services.GetService<ConsoleReporting>();

        // Define the cancellation token that will govern over both tasks
        using (CancellationTokenSource source = new())
        {
            CancellationToken token = source.Token;

            TaskFactory factory = new(token);

            // Prepare 2 tasks
            var taskArray = new Task[2];

            taskArray[0] = await factory.StartNew(async () =>
            {
                await listener.ListenAsync(source);
            }, token);

            taskArray[1] = factory.StartNew(() =>
            {
                reporting.ReportStatistics(token);
            }, token);

            try
            {
                Task.WaitAll(taskArray);
            }
            catch (AggregateException ae)
            {
                foreach (Exception e in ae.InnerExceptions)
                {
                    if (e is TaskCanceledException)
                        Console.WriteLine("Unable to Stream Tweets: {0}",
                           ((TaskCanceledException)e).Message);
                    else
                        Console.WriteLine("Exception: " + e.Message);
                }
            }
        }

        Log.Logger.Information("Application Shutdown.");
    }
}