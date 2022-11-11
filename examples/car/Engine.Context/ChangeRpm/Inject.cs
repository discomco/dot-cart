using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using Engine.Context.Common;
using Engine.Context.Common.Drivers;
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
            .AddTopicMediator()
            .AddSingleton<IProjectionDriver<Common.Schema.Engine>, EngineProjectionDriver>()
            .AddEngineMemStore()
            .AddTransient(_ => Mappers._evt2State)
            .AddHostedService<Effects.ToMemDocProjection>();
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
            .AddHostedService<Effects.Responder>();
    }
}