using DotCart.Abstractions.Actors;
using Engine.Context.Common.Schema;
using Engine.Context.Initialize;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Common.Effects;

public static class Inject
{
    public static IServiceCollection AddESDBEngineEventFeeder(this IServiceCollection services)
    {
        return services
            .AddModelCtor()
            .AddInitializeEngineWithThrottleUpStream()
            .AddTransient<IActor<Spoke>, ESDBEngineEventFeeder>()
            .AddTransient<IESDBEngineEventFeeder, ESDBEngineEventFeeder>();
    }
}