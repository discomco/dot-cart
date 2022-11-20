using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Context.Spokes;
using DotCart.Drivers.InMem;
using DotCart.Drivers.Mediator;
using DotCart.Drivers.NATS;
using DotCart.Drivers.Redis;
using Engine.Context.Common;
using Engine.Context.Initialize;
using Engine.Contract.ChangeRpm;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.ChangeRpm;

public static class Inject
{
    public static IServiceCollection AddChangeRpmBehavior(this IServiceCollection services)
    {
        return services
            .AddModelAggregate()
            .AddAggregateBuilder()
            .AddTransient<ITry, TryCmd>()
            .AddTransient<IApply, ApplyEvt>();
    }

    public static IServiceCollection AddChangeRpmActors(this IServiceCollection services)
    {
        return services
            .AddChangeRpmToRedis()
            .AddChangeRpmBehavior()
            .AddChangeRpmMemProjections()
            .AddChangeRpmEmitter()
            .AddChangeRpmResponder();
    }

    public static IServiceCollection AddChangeRpmMemProjections(this IServiceCollection services)
    {
        return services
            .AddSingletonExchange()
            .AddTransient<IModelStore<Common.Schema.Engine>, MemStore<Common.Schema.Engine>>()
            .AddTransient(_ => Mappers._evt2State)
            .AddTransient<Actors.IToMemDoc, Actors.ToMemDoc>()
            .AddTransient<IActor<Spoke>, Actors.ToMemDoc>();
    }

    public static IServiceCollection AddChangeRpmToRedis(this IServiceCollection services)
    {
        return services
            .AddSingletonExchange()
            .AddTransientRedisDb<Common.Schema.Engine>()
            .AddTransient(_ => Mappers._evt2State)
            .AddTransient<Actors.IToRedisDoc, Actors.ToRedisDoc>()
            .AddTransient<IActor<Spoke>, Actors.ToRedisDoc>();
    }

    public static IServiceCollection AddChangeRpmSpoke(this IServiceCollection services)
    {
        return services
            .AddTransient<Spoke>()
            .AddTransient<ISpokeBuilder<Spoke>, SpokeBuilder>()
            .AddHostedService(provider =>
            {
                var builder = provider.GetRequiredService<ISpokeBuilder<Spoke>>();
                return builder.Build();
            })
            .AddChangeRpmActors();
    }


    public static IServiceCollection AddChangeRpmEmitter(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Mappers._evt2Fact);
    }

    public static IServiceCollection AddChangeRpmMappers(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Mappers._evt2Fact)
            .AddTransient(_ => Mappers._evt2State)
            .AddTransient(_ => Mappers._hope2Cmd);
    }
    
    

    public static IServiceCollection AddChangeRpmResponder(this IServiceCollection services)
    {
        return services
            .AddAggregateBuilder()
            .AddCmdHandler()
            .AddChangeRpmMappers()
            .AddTransient(_ => Generators._generateHope)
            .AddCoreNATS()
            .AddTransient<Actors.IResponder, Actors.Responder>();
    }
}