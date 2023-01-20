using DotCart.Abstractions.Actors;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.TestUtils;

public static partial class Inject
{
    public static IServiceCollection AddTestDocIDCtor(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Schema.DocIDCtor);
    }

    public static IServiceCollection AddTestListCtors(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Schema.ListIDCtor)
            .AddTransient(_ => Schema.FilledListCtor);
    }

    public static IServiceCollection AddTestDocCtors(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Schema.DocIDCtor)
            .AddTransient(_ => Schema.DocCtor);
    }


    public static IServiceCollection AddEventFeeder(this IServiceCollection services)
    {
        return services
            .AddRootDocCtors()
            .AddInitializeWithChangeRpmEvents()
            .AddTransient<IActorB, EventFeeder>();
    }
}