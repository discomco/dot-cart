using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using Engine.Context.Common;
using Engine.Context.Common.Drivers;
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

    public static IServiceCollection AddStartEffects(this IServiceCollection services)
    {
        return services
            .AddStartedProjections()
            .AddStartEmitter()
            .AddStartResponder();
    }

    public static IServiceCollection AddStartEmitter(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Effects._evt2Fact);
    }

    public static IServiceCollection AddStartedProjections(this IServiceCollection services)
    {
        return services
            .AddTopicMediator()
            .AddTransient(_ => Effects._evt2State)
            .AddEngineMemStore()
            .AddSingleton<IProjectionDriver<Common.Schema.Engine>, EngineProjectionDriver>()
            .AddTransient<Effects.IToMemDocProjection, Effects.ToMemDocProjection>()
            .AddTransient<IReactor<Spoke>, Effects.ToMemDocProjection>();
    }

    public static IServiceCollection AddStartResponder(this IServiceCollection services)
    {
        return services
            .AddAggregateBuilder()
            .AddEngineAggregate()
            .AddStartOnInitializedPolicy()
            .AddCmdHandler()
            .AddStartHopeGenerator()
            .AddTransient(_ => Effects._hope2Cmd)
            .AddSingleton<IResponderDriver<Hope>, Effects.ResponderDriver>()
            .AddTransient<Effects.IResponder, Effects.Responder>()
            .AddTransient<IReactor<Spoke>,Effects.Responder>();
    }


    public static IServiceCollection AddStartHopeGenerator(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Generators._generateHope);
    }
}