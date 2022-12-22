using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TwitterStreamingLib.Abstraction;
using TwitterStreamingLib.Configuration;
using TwitterStreamingLib.Core;
using TwitterStreamingLib.DataStructures;

namespace TwitterStreamConsole;

public class Program
{
    private static void BuildConfig(IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }

    /*
    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var listenerConfiguration = new ListenerConfiguration(configuration);
        services.AddSingleton<ListenerConfiguration>(listenerConfiguration);

        
    }

    */

    static async Task Main(string[] args)
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

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // This section needs to be a singleton because the in-memory management of the data
                services.AddSingleton<HashtagAppearances>();
                services.AddSingleton<ITweetRepository, TweetRepository>();

                // Regular transient - instantiate, dispose as many times as necessary. In this case only once.
                services.AddTransient<ITweetHandler, TweetHandler>();
                services.AddTransient<ITwitterStreamListener, TwitterStreamListener>();
            })
            .UseSerilog()
            .Build();

        var listener = host.Services.GetService<ITwitterStreamListener>();
        await listener.ListenAsync();

        Log.Logger.Information("Application Shutdown.");
    }
}