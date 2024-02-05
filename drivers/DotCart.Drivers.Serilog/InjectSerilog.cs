using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace DotCart.Drivers.Serilog;

public static class InjectSerilog
{
    [Obsolete("Use AddSeriloggersFromSettings instead.")]
    public static IServiceCollection AddSeriloggers(this IServiceCollection services,
        bool enableSelfLog = false,
        string appLogPrefix = "default",
        string appLogDir = "")
    {
        services
            .TryAddSingleton(_ => AddLoggerFromSettings(
                enableSelfLog,
                appLogPrefix,
                appLogDir));
        return services;
    }


    public static IServiceCollection AddSeriloggersFromSettings(this IServiceCollection services,
        bool enableSelfLog = false,
        string appLogPrefix = "default",
        string appLogDir = "")
    {
        services
            .TryAddSingleton(_ => AddLoggerFromSettings(
                enableSelfLog,
                appLogPrefix,
                appLogDir));
        return services;
    }


    public static IServiceCollection AddSeriloggersFromCode(this IServiceCollection services,
        bool enableSelfLog = false,
        string appLogPrefix = "default",
        string appLogDir = "")
    {
        services
            .TryAddSingleton(_ => AddLoggerFromCode(
                enableSelfLog,
                appLogPrefix,
                appLogDir));
        return services;
    }


    public static ILogger AddLoggerFromCode(
        bool enableSelfLog = false,
        string appLogPrefix = "default",
        string appLogDir = "")
    {
        if (enableSelfLog)
        {
            SelfLog.Enable(msg => Debug.WriteLine(msg));
            SelfLog.Enable(Console.Error);
        }

        var logFileTemplate = $"logs/{appLogPrefix}-.log";
        if (!string.IsNullOrWhiteSpace(appLogDir)) logFileTemplate = $"logs/{appLogDir}/{appLogPrefix}-.log";

        var logConfig = new LoggerConfiguration();
#pragma warning disable CA1305
        logConfig
            .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithProcessName()
            .WriteTo.File(
                shared: true,
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                restrictedToMinimumLevel: LogEventLevel.Verbose,
                outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{ThreadId:d3}][{Level:u3}] {Message:lj}{NewLine}{Exception}",
                path: $"{logFileTemplate}"
            )
            .WriteTo.Console(
                LogEventLevel.Verbose,
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{ThreadId:d3}][{Level:u3}] {Message:lj}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Code
            );
#pragma warning restore CA1305
        Log.Logger = logConfig
            .CreateLogger();
        return Log.Logger;
    }


    public static ILogger AddLoggerFromSettings(
        bool enableSelfLog = false,
        string appLogPrefix = "default",
        string appLogDir = "")
    {
        if (enableSelfLog)
        {
            SelfLog.Enable(msg => Debug.WriteLine(msg));
            SelfLog.Enable(Console.Error);
        }

        var appSettings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        var logFileTemplate = $"logs/{appLogPrefix}-.log";
        if (!string.IsNullOrWhiteSpace(appLogDir)) logFileTemplate = $"logs/{appLogDir}/{appLogPrefix}-.log";

        var logConfig = new LoggerConfiguration();
        var dontShowCleanup = "Contains(@m, 'cleanup cycle')";
#pragma warning disable CA1305
        logConfig
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
            .ReadFrom.Configuration(appSettings)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithProcessName()
            .Enrich.With<UtcTimestampEnricher>()
            .Filter.ByExcluding(dontShowCleanup)
            .WriteTo.File(
                shared: true,
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                restrictedToMinimumLevel: LogEventLevel.Verbose,
                // outputTemplate: "{Timestamp:u} {UtcOffset:o} [{ThreadId:d3}][{Level:u3}] {Message:lj}{NewLine}{Exception}",
                //                outputTemplate: "{Timestamp:u} [{ThreadId:d3}][{Level:u3}] {Message:lj}{NewLine}{Exception}",
                path: $"{logFileTemplate}"
            )
            .WriteTo.Console(
                LogEventLevel.Verbose,
                "{Timestamp:u} [{ThreadId:d3}][{Level:u3}] {Message:lj}{NewLine}{Exception}",
                //outputTemplate: "{UtcTimestamp:o} [{ThreadId:d3}][{Level:u3}] {Message:lj}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Code
            );
#pragma warning restore CA1305
        Log.Logger = logConfig
            .CreateLogger();
        return Log.Logger;
    }
}