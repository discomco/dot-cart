using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Context.Behaviors;

public interface IAggregateBuilder
{
    IAggregate Build();
}

public static partial class Inject
{
    public static IServiceCollection AddSingletonAggregateBuilder<TAggregateInfo, TAggregateState>(
        this IServiceCollection services)
        where TAggregateInfo : IAggregateInfoB
        where TAggregateState : IState
    {
        return services
            .AddSingleton<IAggregateBuilder, AggregateBuilder<TAggregateInfo, TAggregateState>>();
    }
}

internal class AggregateBuilder<TInfo, TState> : IAggregateBuilder
    where TInfo : IAggregateInfoB
    where TState : IState
{
    private readonly IEnumerable<IApply> _applies;
    private readonly object _buildMutex = new();
    private readonly IEnumerable<IChoreography> _choreography;
    private readonly StateCtorT<TState> _newState;
    private readonly IEnumerable<ITry> _tries;
    private IAggregate _aggregate;

    public AggregateBuilder(
        StateCtorT<TState> newState,
        IEnumerable<IChoreography> choreography,
        IEnumerable<ITry> tries,
        IEnumerable<IApply> applies)
    {
        _newState = newState;
        _choreography = choreography.DistinctBy(rule => rule.Name);
        _tries = tries.DistinctBy(t => t.CmdType);
        _applies = applies.DistinctBy(t => t.EvtType);
    }

    public IAggregate Build()
    {
        lock (_buildMutex)
        {
            _aggregate = AggregateT<TInfo, TState>.Empty(_newState);
            _aggregate.InjectChoreography(_choreography);
            _aggregate.InjectTryFuncs(_tries);
            _aggregate.InjectApplyFuncs(_applies);
            return _aggregate;
        }
    }
}