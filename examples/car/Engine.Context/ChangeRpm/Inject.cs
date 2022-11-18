using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Context.Spokes;
using DotCart.Drivers.InMem;
using DotCart.Drivers.Mediator;
using DotCart.Drivers.Redis;
using Engine.Context.Common;
using Engine.Contract.ChangeRpm;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.ChangeRpm;

public static class Inject
{
    public static IServiceCollection AddChangeRpmBehavior(this IServiceCollection services)
    {
        return services
            .AddEngineAggregate()
            .AddAggregateBuilder()
            .AddTransient<ITry, TryCmd>()
            .AddTransient<IApply, ApplyEvt>();
    }

    public static IServiceCollection AddChangeRpmActors(this IServiceCollection services)
    {
        return services
            .AddChangeRpmRedisProjections();
            // .AddChangeRpmBehavior()
            // .AddChangeRpmMemProjections()
            // .AddChangeRpmEmitter()
            // .AddChangeRpmResponder();
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

    public static IServiceCollection AddChangeRpmRedisProjections(this IServiceCollection services)
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

    public static IServiceCollection AddChangeRpmResponder(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Mappers._hope2Cmd)
            .AddAggregateBuilder()
            .AddCmdHandler()
            .AddTransient(_ => Generators._generateHope)
            .AddSingleton<IResponderDriverT<Hope>, Actors.ResponderDriver>()
            .AddTransient<Actors.IResponder, Actors.Responder>()
            .AddTransient<IActor<Spoke>, Actors.Responder>();
    }
}