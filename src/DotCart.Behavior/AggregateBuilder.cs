using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Behavior;

public static partial class Inject
{
    public static IServiceCollection AddAggregateBuilder(this IServiceCollection services)
    {
        return services
            .AddTopicMediator()
            .AddTransient<IAggregateBuilder, AggregateBuilder>();
    }
}

internal class AggregateBuilder : IAggregateBuilder
{
    private readonly IAggregate _aggregate;
    public AggregateBuilder(
        IAggregate aggregate,
        IEnumerable<IDomainPolicy> policies
    )
    {
        _aggregate = aggregate;
        _aggregate.InjectPolicies(policies);
    }

    public IAggregate Build()
    {
        return _aggregate;
    }
}

public interface IAggregateBuilder
{
    IAggregate Build();
}