using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Context.Effects;
using DotCart.Contract.Schemas;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Common.Drivers;

public static partial class Inject
{
    public static IServiceCollection AddEngineESDBProjectorDriver<TSpoke>(this IServiceCollection services)
        where TSpoke : ISpokeT<TSpoke>
    {
        return services
            .AddProjector()
            .AddTransient(_ =>
                new SubscriptionFilterOptions(
                    StreamFilter.Prefix($"{IDPrefix.Get<IEngineSubscriptionInfo>()}{IDFuncs.PrefixSeparator}")))
            .AddTransient<IProjectorDriver, EngineESDBProjectorDriver>()
            .AddTransient<IProjectorDriver<IEngineSubscriptionInfo>, EngineESDBProjectorDriver>();
    }
}