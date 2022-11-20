using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Context.Behaviors;

public interface IAggregateBuilder
{
    IAggregate Build();
}

public static class Inject
{
    public static IServiceCollection AddAggregateBuilder(this IServiceCollection services)
    {
        return services
            .AddTransient<IAggregateBuilder, AggregateBuilder>();
    }
}

internal class AggregateBuilder : IAggregateBuilder
{
    private readonly IAggregate _aggregate;
    private readonly IEnumerable<IApply> _applies;
    private readonly IEnumerable<IAggregatePolicy> _policies;
    private readonly IEnumerable<ITry> _tries;

    public AggregateBuilder(
        IAggregate aggregate,
        IEnumerable<IAggregatePolicy> policies,
        IEnumerable<ITry> tries,
        IEnumerable<IApply> applies)
    {
        _aggregate = aggregate;
        _policies = policies;
        _tries = tries;
        _applies = applies;
    }

    public IAggregate Build()
    {
        _aggregate.InjectPolicies(_policies);
        _aggregate.InjectTryFuncs(_tries);
        _aggregate.InjectApplyFuncs(_applies);
        return _aggregate;
    }
}