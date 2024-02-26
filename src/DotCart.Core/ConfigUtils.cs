using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Core;

public static class ConfigUtils
{
    public static IServiceCollection AddSettingsFromAppDirectory(this IServiceCollection services,
        string path = "appsettings.json",
        bool isOptional = true,
        bool reloadOnChange = true)
    {
        return services
            .AddTransient<IConfiguration>(
                _ => new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile(path,
                        isOptional,
                        reloadOnChange)
                    .AddEnvironmentVariables()
                    .Build());
    }

    public static IServiceCollection UseSettings<TOptions>(this IServiceCollection services,
        string sectionId,
        Action<TOptions>? overrideOptions = null)
        where TOptions : class
    {
        var prov = services.BuildServiceProvider();
        var cfg = prov.GetRequiredService<IConfiguration>();
        var section = cfg.GetSection(sectionId);
        services.AddOptions<TOptions>();
        services.Configure<TOptions>(section);
        if (overrideOptions != null) services.Configure(overrideOptions);

        return services;
    }
}