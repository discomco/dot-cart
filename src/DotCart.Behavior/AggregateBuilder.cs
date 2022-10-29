using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Behavior;

public static partial class Inject
{
    public static IServiceCollection AddAggregateBuilder(this IServiceCollection services)
    {
        return services
            .AddTopicPubSub()
            .AddTransient<IAggregateBuilder, AggregateBuilder>();
    }
}



internal class AggregateBuilder : IAggregateBuilder
{
    public AggregateBuilder(
        IAggregate aggregate,
        IEnumerable<IDomainPolicy> policies
    )
    {
        aggregate.InjectPolicies(policies);
    }
}

public interface IAggregateBuilder
{}