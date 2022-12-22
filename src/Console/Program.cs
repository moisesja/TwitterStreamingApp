using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        // This section needs to be a singleton because the in-memory management of the data
        services.AddSingleton<HashtagAppearances>();
        services.AddSingleton<ITweetRepository, TweetRepository>();

        services.AddTransient<ITweetHandler, TweetHandler>();
        services.AddTransient<ITwitterStreamListener, TwitterStreamListener>();
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

//        var listener = serviceProvider.GetService<ITwitterStreamListener>();
  //      await listener.ListenAsync();

        Console.WriteLine("Done!!!");
    }
}