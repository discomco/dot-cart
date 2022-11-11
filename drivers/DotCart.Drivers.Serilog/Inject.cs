using System.Diagnostics;
using DotCart.Core;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace DotCart.Drivers.Serilog;

public static class Inject
{
    public static IServiceCollection AddConsoleLogger(this IServiceCollection services, bool enableSelfLog = false)
    {
        return services?
            .AddSingleton(x => CreateSeriLogConsoleLogger(enableSelfLog));
    }

    public static IServiceCollection AddElasticSearchLogger(this ServiceCollection services,
        ElasticsearchSinkOptions options = null,
        bool enableSelfLog = false)
    {
        return services?
            .AddSingleton(x => CreateSeriLogElasticSearchLogger(options, enableSelfLog));
    }


    public static ILogger CreateSeriLogConsoleLogger(bool enableSelfLog = false)
    {
        var level = DotEnv.Get(EnVars.LOG_LEVEL_MIN);
        if (enableSelfLog)
        {
            SelfLog.Enable(msg => Debug.WriteLine(msg));
            SelfLog.Enable(Console.Error);
        }

        Log.Logger = new LoggerConfiguration()
//            .MinimumLevel.Verbose()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
            .MinimumLevel.ControlledBy(new EnvLogLevelSwitch(EnVars.LOG_LEVEL_MIN))
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .WriteTo.Console(LogEventLevel.Verbose,
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message} {NewLine}[{Properties}]{NewLine}{Exception}")
            .CreateLogger();
        return Log.Logger;
    }

    /// <summary>
    ///     Adds Serilog for Elasticsearch.
    ///     Please visit https://github.com/serilog/serilog-aspnetcore for more info
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <param name="enableSelfLog"></param>
    /// <returns></returns>
    private static ILogger CreateSeriLogElasticSearchLogger(ElasticsearchSinkOptions options = null,
        bool enableSelfLog = false)
    {
        if (enableSelfLog)
        {
            SelfLog.Enable(msg => Debug.WriteLine(msg));
            SelfLog.Enable(Console.Error);
        }

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.ControlledBy(new EnvLogLevelSwitch(EnVars.LOG_LEVEL_MIN))
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .WriteTo.Elasticsearch(options)
            .CreateLogger();
        return Log.Logger;
    }
}