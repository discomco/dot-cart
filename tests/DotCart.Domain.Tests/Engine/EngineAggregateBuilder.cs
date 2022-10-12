using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Domain.Tests.Engine;

public static partial class Inject
{
    public static IServiceCollection AddAggregateBuilder(this IServiceCollection services)
    {
        return services
            .AddEngineAggregate()
            .AddTransient<IEngineAggregateBuilder, EngineAggregateBuilder>();
    }
}

public interface IEngineAggregateBuilder : IAggregateBuilder<IEngineAggregate>
{
}

public interface IAggregateBuilder<out T> where T : IAggregate
{
    T Aggregate { get; }
}

public class EngineAggregateBuilder : IEngineAggregateBuilder
{
    public IEngineAggregate Aggregate { get; }

    public EngineAggregateBuilder(
        IEngineAggregate aggregate,
        IEnumerable<IEnginePolicy> policies
    )
    {
        Aggregate = aggregate;
        aggregate.InjectPolicies(policies);
    }
}