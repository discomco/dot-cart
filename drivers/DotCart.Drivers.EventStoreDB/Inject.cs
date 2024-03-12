using DotCart.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.EventStoreDB;

public static partial class Inject
{
    public static IServiceCollection AddESDBSettingsFromAppDirectory(this IServiceCollection services,
        Action<ESDBSettings>? overrideSettings = null)
    {
        return services
            .AddSettingsFromAppDirectory()
            .AddSettingsFromSection(ESDBSettings.SectionId, overrideSettings);
    }






}