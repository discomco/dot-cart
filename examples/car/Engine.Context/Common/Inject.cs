using DotCart.Abstractions.Behavior;
using DotCart.Context.Behaviors;
using DotCart.Drivers.Mediator;
using Engine.Context.Common.Schema;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Common;

public static partial class Inject
{
    public static IServiceCollection AddModelAggregate(this IServiceCollection services)
    {
        return services
            .AddSingletonExchange()
            .AddModelCtor()
            .AddModelIDCtor()
            .AddAggregateBuilder()
            .AddTransient<IAggregate, Aggregate>();
    }
}