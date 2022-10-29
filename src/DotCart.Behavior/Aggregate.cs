using DotCart.Contract;
using DotCart.Schema;

namespace DotCart.Behavior;

public delegate IAggregate AggCtor();

public delegate IAggregate AggBuilder(AggCtor newAgg);

public interface IAggregate
{
    bool IsNew { get; }
    string Name { get; }
    long Version { get; }
    IEnumerable<IEvt> UncommittedEvents { get; }
    string Id();
    IID GetID();
    Task<IFeedback> ExecuteAsync(ICmd cmd);
    IState GetState();
    void SetID(IID getId);
    void Load(IEnumerable<IEvt>? events);
    void ClearUncommittedEvents();
    void InjectPolicies(IEnumerable<IDomainPolicy> policies);
}

public interface IAggregate<TState, TID> : IAggregate
    where TState : IState
    where TID : IID
{
    public TID ID { get; }

    public IAggregate SetID(TID id);
    // bool IsNew();
    // bool HasSourceId(IID sourceId);
}

public class Aggregate<TState, TID> : IAggregate<TState, TID>, IAggregate where TState : IState
    where TID : ID<TID>
{
    private const long _newVersion = -1;

    private readonly ITopicPubSub _pubSub;

    private readonly ICollection<IEvt> _uncommittedEvents = new LinkedList<IEvt>();

    public ICollection<IEvt> _appliedEvents = new LinkedList<IEvt>();
    private object _mutex = new();

    private IEnumerable<IDomainPolicy> _policies = Array.Empty<IDomainPolicy>();

    protected TState _state;

    private bool _withAppliedEvents = false;

    protected Aggregate(
        NewState<TState> newState,
        ITopicPubSub pubSub)
    {
        _state = newState();
        _pubSub = pubSub;
        Version = _newVersion;
    }

    public string Type { get; set; }

    public void InjectPolicies(IEnumerable<IDomainPolicy> aggregatePolicies)
    {
        foreach (var policy in aggregatePolicies) policy.SetBehavior(this);
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

    public void SetID(IID getId)
    {
        ID = getId as TID;
    }

    public void Load(IEnumerable<IEvt>? events)
    {
        // foreach (var evt in events)
        // {
        //     _state = ApplyEvent(_state, evt, ++Version);
        // }

        _state = events.Aggregate(_state, (state, evt) => ApplyEvent(state, evt, ++Version));
    }

    public TID ID { get; private set; }

    public bool IsNew => Version == -1;
    public long Version { get; set; }

    public IID GetID()
    {
        return ID;
    }

    public string Name { get; }

    public string Id()
    {
        return ID.Value;
    }

    public IAggregate SetID(TID id)
    {
        ID = id;
        return this;
    }


    public async Task<IFeedback> ExecuteAsync(ICmd cmd)
    {
        var fbk = Feedback.New(cmd.GetID());
        try
        {
            fbk = ((dynamic)this).Verify((dynamic)cmd);

            if (!fbk.IsSuccess) return fbk;

            IEnumerable<IEvt> events = ((dynamic)this).Raise((dynamic)cmd);

            foreach (var @event in events) await RaiseEvent(@event);
            fbk.SetPayload(_state);
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsError());
        }

        return fbk;
    }

    private async Task RaiseEvent(IEvt evt)
    {
        _state = ApplyEvent(_state, evt, Version++);
        _uncommittedEvents.Add(evt);
        await _pubSub.PublishAsync(evt.EventType, evt);
    }

    private TState ApplyEvent(TState state, IEvt evt, long version)
    {
        if (_uncommittedEvents.Any(x => Equals(x.EventId, evt.EventId))) return _state;
        Version = version;
        evt.SetVersion(Version);
        return ((dynamic)this).Apply(state, (dynamic)evt);
    }
}