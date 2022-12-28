using DotCart.Abstractions.Actors;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.TestUtils;

public static partial class Inject
{
    public static IServiceCollection AddTestIDCtor(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Schema.DocIDCtor);
    }


    public static IServiceCollection AddEventFeeder(this IServiceCollection services)
    {
        return services
            .AddRootDocCtors()
            .AddInitializeWithChangeRpmEvents()
            .AddTransient<IActor, EventFeeder>();
    }
}