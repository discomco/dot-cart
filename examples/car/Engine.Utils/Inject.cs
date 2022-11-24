using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Utils;

public static partial class Inject
{
    public static IServiceCollection AddEventFeeder(this IServiceCollection services)
    {
        return services
            .AddModelCtor()
            .AddInitializeWithThrottleUpEvents()
            .AddTransient<IEventFeeder, EventFeeder>();
    }
}