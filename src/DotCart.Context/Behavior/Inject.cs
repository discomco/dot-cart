using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Drivers.Serilog;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Context.Behavior;

public static partial class Inject
{
    public static IServiceCollection AddChoreography<TCmdPayload, TEvtPayload, TMeta>(this IServiceCollection services,
        Evt2Cmd<TCmdPayload, TEvtPayload, TMeta> evt2Cmd)
        where TCmdPayload : IPayload
        where TEvtPayload : IPayload
        where TMeta : IMeta
    {
        return services
            .AddTransient<IChoreography, ChoreographyT<TCmdPayload, TEvtPayload, TMeta>>()
            .AddTransient(_ => evt2Cmd);
    }

    public static IServiceCollection AddBaseBehavior<TAggregateInfo, TState, TPayload, TMeta>(
        this IServiceCollection services)
        where TState : IState
        where TAggregateInfo : IAggregateInfoB
        where TPayload : IPayload
        where TMeta : IMeta
    {
        return services
            .AddConsoleLogger()
            .AddSingletonAggregateBuilder<TAggregateInfo, TState>()
            .AddTransient<ITry, TryCmdT<TState, TPayload, TMeta>>()
            .AddTransient<IApply, ApplyEvtT<TState, TPayload, TMeta>>();
    }
}