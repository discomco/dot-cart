using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Context.Spokes;
using DotCart.Drivers.Mediator;
using DotCart.Drivers.Redis;
using Engine.Context.Common;
using Engine.Contract.Start;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Start;

public static class Inject
{
    public static IServiceCollection AddStartBehavior(this IServiceCollection services)
    {
        return services
            .AddEngineAggregate()
            .AddAggregateBuilder()
            .AddTransient<ITry, TryCmd>()
            .AddTransient<IApply, ApplyEvt>();
    }


    public static IServiceCollection AddStartOnInitializedPolicy(this IServiceCollection services)
    {
        return services
            .AddEngineAggregate()
            .AddAggregateBuilder()
            .AddTransient(_ => Mappers.evt2Cmd)
            .AddTransient<IAggregatePolicy, StartOnInitializedPolicy>();
    }

    public static IServiceCollection AddStartActors(this IServiceCollection services)
    {
        return services
            .AddStartedToRedisProjections()
            .AddStartEmitter();
        // .AddStartResponder();
    }

    public static IServiceCollection AddStartEmitter(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Mappers._evt2Fact);
    }

    public static IServiceCollection AddStartedToRedisProjections(this IServiceCollection services)
    {
        return services
            .AddSingletonExchange()
            .AddTransient(_ => Mappers._evt2State).AddTransientRedisDb<Common.Schema.Engine>()
            .AddSingleton<IRedisStore<Common.Schema.Engine>, RedisStore<Common.Schema.Engine>>()
            .AddTransient<IActor<Spoke>, Actors.ToRedisDoc>()
            .AddTransient<Actors.IToRedisDoc, Actors.ToRedisDoc>();
    }


    public static IServiceCollection AddStartResponder(this IServiceCollection services)
    {
        return services
            .AddAggregateBuilder()
            .AddEngineAggregate()
            .AddStartOnInitializedPolicy()
            .AddCmdHandler()
            .AddStartHopeGenerator()
            .AddTransient(_ => Mappers._hope2Cmd)
            .AddSingleton<IResponderDriverT<Hope>, Actors.ResponderDriver>()
            .AddTransient<Actors.IResponder, Actors.Responder>()
            .AddTransient<IActor<Spoke>, Actors.Responder>();
    }


    public static IServiceCollection AddStartHopeGenerator(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Generators._generateHope);
    }


    public static IServiceCollection AddStartSpoke(this IServiceCollection services)
    {
        return services
            .AddStartActors()
            .AddTransient<ISpokeBuilder<Spoke>, SpokeBuilder>()
            .AddTransient<Spoke>()
            .AddHostedService(provider =>
            {
                var spokeBuilder = provider.GetRequiredService<ISpokeBuilder<Spoke>>();
                return spokeBuilder.Build();
            });
    }
}