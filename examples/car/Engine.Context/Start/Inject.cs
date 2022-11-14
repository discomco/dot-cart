using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Drivers.InMem;
using DotCart.Drivers.Mediator;
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
            .AddExchange()
            .AddTransient(_ => Effects._evt2State)
            .AddSingleton<IModelStore<Common.Schema.Engine>, MemStore<Common.Schema.Engine>>()
            .AddTransient<Effects.IToMemDocProjection, Effects.ToMemDocProjection>()
            .AddTransient<IActor<Spoke>, Effects.ToMemDocProjection>();
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
            .AddTransient<IActor<Spoke>, Effects.Responder>();
    }


    public static IServiceCollection AddStartHopeGenerator(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Generators._generateHope);
    }
}