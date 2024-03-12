using DotCart.Core;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.EventStoreDB;

public static partial class Inject
{
    public static IServiceCollection AddESDSettingsFromAppDirectory(this IServiceCollection services,
        Action<ESDBSettings>? overrideSettings = null)
    {
        return services
            .AddSettingsFromAppDirectory()
            .UseSettings(ESDBSettings.SectionId, overrideSettings);
    }
}