using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Drivers.InMem;
using DotCart.Drivers.Mediator;
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

    public static IServiceCollection AddChangeRpmEffects(this IServiceCollection services)
    {
        return services
            .AddChangeRpmBehavior()
            .AddChangeRpmProjections()
            .AddChangeRpmEmitter()
            .AddChangeRpmResponder();
    }

    public static IServiceCollection AddChangeRpmProjections(this IServiceCollection services)
    {
        return services
            .AddExchange()
            .AddTransient<IModelStore<Common.Schema.Engine>, MemStore<Common.Schema.Engine>>()
            .AddTransient(_ => Mappers._evt2State)
            .AddTransient<Effects.IToMemDocProjection, Effects.ToMemDocProjection>()
            .AddTransient<IActor<Spoke>, Effects.ToMemDocProjection>();
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
            .AddSingleton<IResponderDriver<Hope>, Effects.ResponderDriver>()
            .AddTransient<Effects.IResponder, Effects.Responder>()
            .AddTransient<IActor<Spoke>, Effects.Responder>();
    }
}