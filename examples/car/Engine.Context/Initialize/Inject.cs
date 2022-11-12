using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using DotCart.Context.Spokes;
using DotCart.Drivers.InMem;
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
            .AddInitializeEffects();
    }

    public static IServiceCollection AddInitializeEffects(this IServiceCollection services)
    {
        return services
            .AddInitializeResponder()
            .AddInitializedEmitter()
            .AddInitializedMemProjections()
            .AddInitializedRedisProjections();
    }

    public static IServiceCollection AddInitializedRedisProjections(this IServiceCollection services)
    {
        return services
            .AddTopicMediator()
            .AddTransient(_ => Mappers._evt2State)
            .AddTransient<IReactor<Spoke>, Projector<Spoke>>()
            .AddTransient<Effects.IToRedisDoc, Effects.ToRedisDoc>()
            .AddTransient<IReactor<Spoke>, Effects.ToRedisDoc>()
            .AddTransientRedisDb<Common.Schema.Engine>();
    }

    public static IServiceCollection AddInitializedEmitter(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Mappers._evt2Fact);
    }


    public static IServiceCollection AddInitializedMemProjections(this IServiceCollection services)
    {
        return services
            .AddTopicMediator()
            .AddTransient(_ => Mappers._evt2State)
            .AddTransient<IReactor<Spoke>, Projector<Spoke>>()
            .AddTransient<IModelStore<Common.Schema.Engine>, MemStore<Common.Schema.Engine>>()
            .AddTransient<Effects.IMemDocProjection, Effects.ToMemDocProjection>()
            .AddTransient<IReactor<Spoke>, Effects.ToMemDocProjection>();
    }


    public static IServiceCollection AddInitializeResponder(this IServiceCollection services)
    {
        return services
            .AddAggregateBuilder()
            .AddEngineAggregate()
            .AddCmdHandler()
            .AddTransient(_ => Generators._genHope)
            .AddTransient(_ => Mappers._hope2Cmd)
            .AddSingleton<IResponderDriver<Hope>, Drivers.ResponderDriver>()
            .AddTransient<Effects.IResponder, Effects.Responder>()
            .AddTransient<IReactor<Spoke>, Effects.Responder>();
    }
}