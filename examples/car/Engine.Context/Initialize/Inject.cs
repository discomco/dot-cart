using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Context.Spokes;
using DotCart.Drivers.EventStoreDB;
using DotCart.Drivers.InMem;
using DotCart.Drivers.Mediator;
using DotCart.Drivers.NATS;
using DotCart.Drivers.Redis;
using Engine.Context.Common;
using Engine.Contract.Initialize;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Initialize;

public static partial class Inject
{

    public static IServiceCollection AddInitializeMappers(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Mappers._evt2Fact)
            .AddTransient(_ => Mappers._evt2Doc)
            .AddTransient(_ => Mappers._hope2Cmd);
    }
    
    public static IServiceCollection AddInitializeBehavior(this IServiceCollection services)
    {
        return services
            .AddModelAggregate()
            .AddAggregateBuilder()
            .AddTransient<ITry, TryCmd>()
            .AddTransient<IApply, ApplyEvt>();
    }

    public static IServiceCollection AddInitializeSpoke(this IServiceCollection services)
    {
        return services
            .AddTransient<Spoke>()
            .AddSingleton<ISpokeBuilder<Spoke>, SpokeBuilder>()
            .AddHostedService(provider =>
            {
                var spokeBuilder = provider.GetRequiredService<ISpokeBuilder<Spoke>>();
                return spokeBuilder.Build();
            })
            .AddInitializeMappers()
            .AddInitializeActors();
    }

    public static IServiceCollection AddInitializeActors(this IServiceCollection services)
    {
        return services
            .AddInitializeResponders()
            .AddInitializedEmitter()
            .AddInitializedToRedisProjections();
    }

    public static IServiceCollection AddInitializedToRedisProjections(this IServiceCollection services)
    {
        return services
            .AddSingletonExchange()
            .AddTransient(_ => Mappers._evt2Doc)
            .AddTransient<Actors.IToRedisDoc, Actors.ToRedisDoc>()
            .AddTransient<IActor<Spoke>, Actors.ToRedisDoc>()
            .AddTransientRedisDb<Common.Schema.Engine>();
    }

    public static IServiceCollection AddInitializedEmitter(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Mappers._evt2Fact);
    }

    public static IServiceCollection AddInitializedToMemProjections(this IServiceCollection services)
    {
        return services
            .AddSingletonExchange()
            .AddTransient(_ => Mappers._evt2Doc)
            .AddSingleton<IModelStore<Common.Schema.Engine>, MemStore<Common.Schema.Engine>>()
            .AddTransient<Actors.IToMemDoc, Actors.ToMemDoc>()
            .AddTransient<IActor<Spoke>, Actors.ToMemDoc>();
    }

    public static IServiceCollection AddInitializeResponders(this IServiceCollection services)
    {
        return services
            .AddSingletonExchange()
            .AddAggregateBuilder()
            .AddModelAggregate()
            .AddCmdHandler()
            .AddInitializeMappers()
            .AddNATSResponder<Hope, Cmd>()
            .AddSpokedNATSResponder<Spoke, Hope, Cmd>();



    }
}