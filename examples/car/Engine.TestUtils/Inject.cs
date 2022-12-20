using DotCart.Abstractions.Actors;
using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.TestUtils;

public static partial class Inject
{
    public static IServiceCollection AddTestIDCtor(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Schema.IDCtor);
    }


    public static IServiceCollection AddEventFeeder(this IServiceCollection services)
    {
        return services
            .AddStateCtor()
            .AddInitializeWithChangeRpmEvents()
            .AddTransient<IActor, EventFeeder>();
    }
}