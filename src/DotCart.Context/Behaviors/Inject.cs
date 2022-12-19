using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Drivers.Mediator;
using DotCart.Drivers.Serilog;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Context.Behaviors;

public static partial class Inject
{
    public static IServiceCollection AddBaseBehavior<TInfo, TState, TCmd, TEvt>(this IServiceCollection services)
        where TCmd : ICmdB
        where TState : IState
        where TEvt : IEvtB
        where TInfo : IAggregateInfoB
    {
        return services
            .AddConsoleLogger()
            .AddSingletonExchange()
            .AddAggregateBuilder()
            .AddTransient<IAggregate, AggregateT<TInfo, TState>>()
            .AddTransient<ITry, TryCmdT<TCmd, TState>>()
            .AddTransient<IApply, ApplyEvtT<TState, TEvt>>();
    }
}