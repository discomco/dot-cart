using DotCart.Abstractions.Actors;
using Engine.Context.Common.Schema;
using Engine.Context.Initialize;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Common.Effects;

public static class Inject
{
    public static IServiceCollection AddEventFeeder(this IServiceCollection services)
    {
        return services
            .AddModelCtor()
            .AddInitializeWithThrottleUpEvents()
            .AddTransient<IActor<Spoke>, EventFeeder>()
            .AddTransient<IEventFeeder, EventFeeder>();
    }
}