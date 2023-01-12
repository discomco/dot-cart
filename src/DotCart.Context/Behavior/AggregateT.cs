using System.Collections.Immutable;
using Ardalis.GuardClauses;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Serilog;

namespace DotCart.Context.Behavior;

/// <summary>
///     AggregateT
///     <TInfo, TState>
///         is an aggregation class for event sourced State management.
///         Aggregates are composed of ITry, IApply and IChoreography functions and should never be instantiated directly.
///         Construction of AggregateT classes should be done via the AggregateBuilder.Build() method.
/// </summary>
/// <typeparam name="TInfo">A Injection Discriminator that carries additional information about the Aggregate</typeparam>
/// <typeparam name="TState"></typeparam>
internal class AggregateT<TInfo, TState> : IAggregate
    where TState : IState
    where TInfo : IAggregateInfoB
{
    private readonly ICollection<IEvtB>
        _appliedEvents = new LinkedList<IEvtB>();

    private readonly ICollection<IEvtB>
        _uncommittedEvents = new LinkedList<IEvtB>();

    private readonly object execMutex = new();

    private IImmutableDictionary<string, IApply>
        _applyFuncs = ImmutableDictionary<string, IApply>.Empty;

    private IImmutableDictionary<string, IChoreography>
        _choreography = ImmutableDictionary<string, IChoreography>.Empty;

    private ulong _nextVersion;
    protected TState _state;

    private IImmutableDictionary<string, ITry>
        _tryFuncs = ImmutableDictionary<string, ITry>.Empty;


    private bool _withAppliedEvents = false;

    protected AggregateT(StateCtorT<TState> newState)
    {
        _state = newState();
        Version = EventConst.NewAggregateVersion;
    }

    public bool KnowsApply(string evtType)
    {
        return _applyFuncs.ContainsKey(evtType);
    }

    public void InjectTryFuncs(IEnumerable<ITry> tryFuncs)
    {
        _tryFuncs =
            tryFuncs
                .DistinctBy(
                    f => f.CmdType
                )
                .Select<ITry, KeyValuePair<string, ITry>>(
                    x
                        => new KeyValuePair<string, ITry>(
                            x.CmdType,
                            x.SetAggregate(this)
                        )
                )
                .ToImmutableDictionary();
    }

    public void InjectApplyFuncs(IEnumerable<IApply> applyFuncs)
    {
        _applyFuncs =
            applyFuncs
                .DistinctBy(f
                    => f.EvtType
                )
                .Select<IApply, KeyValuePair<string, IApply>>(f
                    => new KeyValuePair<string, IApply>(f.EvtType, f.SetAggregate(this))
                )
                .ToImmutableDictionary();
    }

    public bool KnowsTry(string cmdType)
    {
        return _tryFuncs.ContainsKey(cmdType);
    }

    public bool KnowsChoreography(string name)
    {
        return _choreography.Any(s => s.Key == name);
    }

    public void InjectChoreography(IEnumerable<IChoreography> choreography)
    {
        _choreography =
            choreography
                .DistinctBy(c
                    => c.Name
                )
                .Select<IChoreography, KeyValuePair<string, IChoreography>>(c
                    => new KeyValuePair<string, IChoreography>(c.Name, c.SetAggregate(this))
                )
                .ToImmutableDictionary();
    }

    public string GetName()
    {
        return NameAtt.Get<TInfo>();
    }

    public void ClearUncommittedEvents()
    {
        _uncommittedEvents.Clear();
    }

    public IEnumerable<IEvtB> UncommittedEvents => _uncommittedEvents;

    public IState GetState()
    {
        return _state;
    }

    public MetaB GetMeta()
    {
        return MetaB.New(GetName(), GetID().Id());
    }

    public void ClearUncommittedEvents(ulong resNextExpectedVersion)
    {
        ClearUncommittedEvents();
        _nextVersion = resNextExpectedVersion;
    }


    public IAggregate SetID(IID ID)
    {
        this.ID = ID;
        return this;
    }

    public void Load(IEnumerable<IEvtB>? events)
    {
        try
        {
            Guard.Against.BehaviorIDNotSet(this);
            _state = events.Aggregate(_state, ApplyEvent);
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            throw;
        }
    }

    public IID ID { get; private set; }
    public bool IsNew => Version == -1;
    public long Version { get; set; }


    public string Id()
    {
        return ID.Id();
    }

    public async Task<Feedback> ExecuteAsync(ICmdB cmd, IFeedback previous)
    {
        var feedback = Feedback.New(cmd.AggregateID.Id(), previous);
        try
        {
            Guard.Against.Null(cmd);
            Guard.Against.Null(cmd.AggregateID);
            SetID(cmd.AggregateID);
            var fTry = _tryFuncs[cmd.CmdType];
            feedback = ((dynamic)fTry).Verify((dynamic)cmd, (dynamic)_state);
            if (!feedback.IsSuccess)
                return feedback;
            IEnumerable<IEvtB> events = ((dynamic)fTry).Raise((dynamic)cmd, (dynamic)_state);
            foreach (var @event in events)
                await RaiseEvent(@event);
            feedback.SetPayload(_state);
        }
        catch (Exception e)
        {
            feedback.SetError(e.AsError());
        }

        return feedback;
    }

    public static IAggregate Empty(StateCtorT<TState> newState)
    {
        return new AggregateT<TInfo, TState>(newState);
    }

    public IID GetID()
    {
        return ID;
    }

    private async Task RaiseEvent(IEvtB evt)
    {
        evt.SetVersion(Version + 1);
        evt.SetTimeStamp(DateTime.UtcNow);
        _state = ApplyEvent(_state, evt);
        _uncommittedEvents.Add(evt);
        await WhenAsync(evt);
    }

    private async Task WhenAsync(IEvtB evt)
    {
        var steps = _choreography.Where(c => c.Value.Topic == evt.Topic);
        foreach (var step in steps)
        {
            step.Value.SetAggregate(this);
            await step.Value.WhenAsync(evt);
        }
    }


    private TState ApplyEvent(TState state, IEvtB evt)
    {
        if (_uncommittedEvents.Any(x => Equals(x.EventId, evt.EventId)))
            return _state;
        Version = evt.Version;
        var applyFunc = _applyFuncs[evt.Topic];
        var newState = ((dynamic)applyFunc).Apply(state, (dynamic)evt);
        _appliedEvents.Add(evt);
        return newState;
    }
}