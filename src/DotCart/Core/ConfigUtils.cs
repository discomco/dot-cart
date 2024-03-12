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

    public static IServiceCollection AddSettingsFromSection<TSettings>(this IServiceCollection services,
        string sectionId,
        Action<TSettings>? overrideSettings = null)
        where TSettings : class
    {
        var prov = services.BuildServiceProvider();
        var cfg = prov.GetRequiredService<IConfiguration>();
        var section = cfg.GetSection(sectionId);
        services.AddOptions<TSettings>();
        services.Configure<TSettings>(section);
        if (overrideSettings != null) services.Configure(overrideSettings);

        return services;
    }
}