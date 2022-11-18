using DotCart.Context.Abstractions;
using DotCart.Drivers.Mediator;
using Engine.Contract.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Common;

public static partial class Inject
{
    public static IServiceCollection AddEngineAggregate(this IServiceCollection services)
    {
        return services
            .AddSingletonExchange()
            .AddSingleton(Schema.Engine.Ctor)
            .AddSingleton(EngineID.Ctor)
            .AddTransient<IAggregate, Aggregate>();
    }
}