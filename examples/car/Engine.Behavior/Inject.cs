using DotCart.Abstractions.Behavior;
using DotCart.Context.Behaviors;
using DotCart.Core;
using DotCart.Drivers.Mediator;
using DotCart.Drivers.Serilog;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Behavior;

public static partial class Inject
{

    public static IServiceCollection AddEngineBehavior(this IServiceCollection services)
    {
        return services
            .AddInitializeBehavior()
            .AddStartBehavior()
            .AddChangeRpmBehavior()
            .AddStopBehavior();
    }

    public static IServiceCollection AddBaseBehavior(this IServiceCollection services)
    {
        return services
            .AddConsoleLogger()
            .AddSingletonExchange()
            .AddAggregateBuilder()
            .AddTransient<IAggregate, Aggregate>()
            .AddStateCtor();
    }
}