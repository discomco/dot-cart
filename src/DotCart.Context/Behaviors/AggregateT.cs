using System.Collections.Immutable;
using Ardalis.GuardClauses;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Serilog;

namespace DotCart.Context.Behaviors;

public class AggregateT<TState> : IAggregate
    where TState : IState
{
    private readonly ICollection<IEvt> _uncommittedEvents = new LinkedList<IEvt>();

    private readonly object execMutex = new();
    public ICollection<IEvt> _appliedEvents = new LinkedList<IEvt>();
    private IImmutableDictionary<string, IApply> _applyFuncs = ImmutableDictionary<string, IApply>.Empty;
    private ulong _nextVersion;
    protected TState _state;
    private IImmutableDictionary<string, ITry> _tryFuncs = ImmutableDictionary<string, ITry>.Empty;
    private bool _withAppliedEvents = false;

    protected AggregateT(
        StateCtorT<TState> newState)
    {
        _state = newState();
        Version = Constants.NewAggregateVersion;
    }

    public bool KnowsApply(string evtType)
    {
        return _applyFuncs.ContainsKey(evtType);
    }

    public void InjectTryFuncs(IEnumerable<ITry> tryFuncs)
    {
        foreach (var fTry in tryFuncs)
        {
            if (KnowsTry(fTry.CmdType)) continue;
            fTry.SetAggregate(this);
            _tryFuncs = _tryFuncs.Add(fTry.CmdType, fTry);
        }
    }

    public void InjectApplyFuncs(IEnumerable<IApply> applyFuncs)
    {
        foreach (var apply in applyFuncs)
        {
            if (KnowsApply(apply.EvtType)) continue;
            apply.SetAggregate(this);
            _applyFuncs = _applyFuncs.Add(apply.EvtType, apply);
        }
    }

    public bool KnowsTry(string cmdType)
    {
        return _tryFuncs.ContainsKey(cmdType);
    }

    public void InjectPolicies(IEnumerable<IAggregatePolicy> aggregatePolicies)
    {
        foreach (var policy in aggregatePolicies)
            policy.SetBehavior(this);
    }

    public string GetName()
    {
        return GetType().FullName;
    }

    public void ClearUncommittedEvents()
    {
        _uncommittedEvents.Clear();
    }

    public IEnumerable<IEvt> UncommittedEvents => _uncommittedEvents;

    public IState GetState()
    {
        return _state;
    }

    public EventMeta GetMeta()
    {
        return EventMeta.New(GetName(), GetID().Id());
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

    public void Load(IEnumerable<IEvt>? events)
    {
        try
        {
            Guard.Against.BehaviorIDNotSet(this);
            _state = events.Aggregate(_state, (state, evt) => ApplyEvent(state, evt, evt.Version));
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

    public async Task<Feedback> ExecuteAsync(ICmd cmd)
    {
        var feedback = Feedback.New(cmd.AggregateID.Id());
        try
        {
            Guard.Against.Null(cmd);
            Guard.Against.Null(cmd.AggregateID);
            SetID(cmd.AggregateID);
            Guard.Against.BehaviorIDNotSet(this);
            var fTry = _tryFuncs[TopicAtt.Get(cmd)];
            feedback = ((dynamic)fTry).Verify((dynamic)cmd);
            if (!feedback.IsSuccess) return feedback;
            IEnumerable<IEvt> events = ((dynamic)fTry).Raise((dynamic)cmd);
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

    public IID GetID()
    {
        return ID;
    }

    private async Task RaiseEvent(IEvt evt)
    {
        evt.Version = Version + 1;
        // if (Version >= evt.Version) 
        //     return;
        evt.SetTimeStamp(DateTime.UtcNow);
        _state = ApplyEvent(_state, evt, evt.Version);
//        _state = ApplyEvent(_state, evt, ++Version);
        _uncommittedEvents.Add(evt);
        //await _mediator.PublishAsync(evt.Topic, evt);
    }


    private TState ApplyEvent(TState state, IEvt evt, long version)
    {
        if (_uncommittedEvents.Any(x => Equals(x.EventId, evt.EventId)))
            return _state;
        Version = evt.Version;
//        evt.Version = version;
//        Version = evt.Version;
        // evt.Version = Version;
        var applyFunc = _applyFuncs[evt.Topic];
        var newState = ((dynamic)applyFunc).Apply(state, (dynamic)evt);
        _appliedEvents.Add(evt);
        return newState;
    }
}