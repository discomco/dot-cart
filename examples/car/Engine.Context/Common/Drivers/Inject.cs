using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using DotCart.Contract.Schemas;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Common.Drivers;

public static partial class Inject
{
    public static IServiceCollection AddEngineMemProjectionDriver(this IServiceCollection services)
    {
        return services
            .AddEngineMemStore()
            .AddSingleton<IEngineMemProjectionDriver, EngineProjectionDriver>()
            .AddSingleton<IProjectionDriver<Schema.Engine>, EngineProjectionDriver>();
    }
    public static IServiceCollection AddEngineMemStore(this IServiceCollection services)
    {
        return services
            .AddSingleton<IModelStoreDriver<Schema.Engine>, EngineMemModelStoreDriver>()
            .AddSingleton<IEngineModelStoreDriver, EngineMemModelStoreDriver>();
    }
    public static IServiceCollection AddEngineESDBProjectorDriver<TSpoke>(this IServiceCollection services) where TSpoke : ISpoke<TSpoke>
    {
        return services
            .AddProjector<TSpoke>()
            .AddTransient(_ =>
                new SubscriptionFilterOptions(
                    StreamFilter.Prefix($"{IDPrefix.Get<IEngineSubscriptionInfo>()}{IDFuncs.PrefixSeparator}")))
            .AddTransient<IProjectorDriver, EngineESDBProjectorDriver>()
            .AddTransient<IProjectorDriver<IEngineSubscriptionInfo>, EngineESDBProjectorDriver>();
    }
}