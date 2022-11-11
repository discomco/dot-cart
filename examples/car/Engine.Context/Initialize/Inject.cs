using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using Engine.Context.Common;
using Engine.Context.Common.Drivers;
using Engine.Contract.Initialize;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Initialize;

public static class Inject
{
    public static IServiceCollection AddInitializeBehavior(this IServiceCollection services)
    {
        return services
            .AddEngineAggregate()
            .AddAggregateBuilder()
            .AddTransient<ITry, TryCmd>()
            .AddTransient<IApply, ApplyEvt>();
    }

    public static IServiceCollection AddInitializeEffects(this IServiceCollection services)
    {
        return services
            .AddInitializeResponder()
            .AddInitializedEmitter()
            .AddInitializedProjections();
    }

    public static IServiceCollection AddInitializedEmitter(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Effects._evt2Fact);
    }


    public static IServiceCollection AddInitializedProjections(this IServiceCollection services)
    {
        return services
            .AddTopicMediator()
            .AddEngineMemStore()
            .AddSingleton(_ => Effects._evt2State)
            .AddSingleton<IProjectionDriver<Common.Schema.Engine>, EngineProjectionDriver>()
            .AddHostedService<Effects.ToMemDocProjection>();
    }


    public static IServiceCollection AddInitializeResponder(this IServiceCollection services)
    {
        return services
            .AddAggregateBuilder()
            .AddEngineAggregate()
            .AddCmdHandler()
            .AddTransient(_ => Effects._genHope)
            .AddTransient(_ => Effects._hope2Cmd)
            .AddSingleton<IResponderDriver<Hope>, Effects.ResponderDriver>()
            .AddHostedService<Effects.Responder>();
    }
}