using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using DotCart.Contract.Schemas;
using DotCart.Drivers.InMem;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Common.Drivers;

public static partial class Inject
{

    public static IServiceCollection AddEngineESDBProjectorDriver<TSpoke>(this IServiceCollection services)
        where TSpoke : ISpoke<TSpoke>
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