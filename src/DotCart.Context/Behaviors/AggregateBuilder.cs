using System.Collections.Immutable;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Context.Behaviors;

public interface IAggregateBuilder
{
    IAggregate Build();
}

public static partial class Inject
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
    private readonly IEnumerable<IChoreography> _rules;
    private readonly IEnumerable<ITry> _tries;

    public AggregateBuilder(
        IAggregate aggregate,
        IEnumerable<IChoreography> choreography,
        IEnumerable<ITry> tries,
        IEnumerable<IApply> applies)
    {
        _aggregate = aggregate;
        _rules = Distinct(choreography);
        _tries = tries;
        _applies = applies;
    }

    public IAggregate Build()
    {
        _aggregate.InjectChoreography(_rules);
        _aggregate.InjectTryFuncs(_tries);
        _aggregate.InjectApplyFuncs(_applies);
        return _aggregate;
    }

    private static IEnumerable<IChoreography> Distinct(IEnumerable<IChoreography> rules)
    {
        var result = ImmutableList<IChoreography>.Empty;
        var known = new List<string>();
        foreach (var rule in rules)
        {
            if (known.Contains(rule.Name)) continue;
            known.Add(rule.Name);
            result = result.Add(rule);
        }
        return result;
    }
}