using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Context.Spokes;
using DotCart.Drivers.InMem;
using DotCart.Drivers.Mediator;
using DotCart.Drivers.Redis;
using Engine.Context.Common;
using Engine.Contract.Initialize;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Initialize;

public static partial class Inject
{
    public static IServiceCollection AddInitializeBehavior(this IServiceCollection services)
    {
        return services
            .AddEngineAggregate()
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
            .AddInitializeActors();
    }

    public static IServiceCollection AddInitializeActors(this IServiceCollection services)
    {
        return services
            .AddInitializeResponder()
            .AddInitializedEmitter()
            .AddInitializedRedisProjections();
    }

    public static IServiceCollection AddInitializedRedisProjections(this IServiceCollection services)
    {
        return services
            .AddSingletonExchange()
            .AddTransient(_ => Mappers._evt2State)
            .AddTransient<Actors.IToRedisDoc, Actors.ToRedisDoc>()
            .AddTransient<IActor<Spoke>, Actors.ToRedisDoc>()
            .AddTransientRedisDb<Common.Schema.Engine>();
    }

    public static IServiceCollection AddInitializedEmitter(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Mappers._evt2Fact);
    }

    public static IServiceCollection AddInitializedProjections(this IServiceCollection services)
    {
        return services
            .AddSingletonExchange()
            .AddTransient(_ => Mappers._evt2State)
            .AddSingleton<IModelStore<Common.Schema.Engine>, MemStore<Common.Schema.Engine>>()
            .AddTransient<Actors.IToMemDoc, Actors.ToMemDoc>()
            .AddTransient<IActor<Spoke>, Actors.ToMemDoc>();
    }

    public static IServiceCollection AddInitializeResponder(this IServiceCollection services)
    {
        return services
            .AddSingletonExchange()
            .AddAggregateBuilder()
            .AddEngineAggregate()
            .AddCmdHandler()
            .AddTransient(_ => Generators._genHope)
            .AddTransient(_ => Mappers._hope2Cmd)
            .AddSingleton<IResponderDriverT<Hope>, Drivers.ResponderDriver>()
            .AddTransient<Actors.IResponder, Actors.Responder>()
            .AddTransient<IActor<Spoke>, Actors.Responder>();
    }
}