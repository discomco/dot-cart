using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using Engine.Behavior;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Utils;

public static partial class Inject
{


    public static IServiceCollection AddTestIDCtor(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Utils.Schema.TestIDCtor);
    }
    
    
    public static IServiceCollection AddEventFeeder(this IServiceCollection services)
    {
        return services
            .AddStateCtor()
            .AddInitializeWithChangeRpmEvents()
            .AddTransient<IActor, EventFeeder>();
    }
}