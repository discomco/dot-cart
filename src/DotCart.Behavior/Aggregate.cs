using DotCart.Contract;
using DotCart.Schema;

namespace DotCart.Behavior;

public delegate IAggregate AggCtor();

public delegate IAggregate AggBuilder(AggCtor newAgg);

public interface IAggregate
{
    bool IsNew { get; }
    string Name { get; }
    string Id();
    long Version { get; }
    IID GetID();
    Task<IFeedback> ExecuteAsync(ICmd cmd);
    IState GetState();
    void SetID(IID getId);
    void Load(IEnumerable<IEvt>? events);
    void ClearUncommittedEvents();
    IEnumerable<IEvt> UncommittedEvents { get; }
    void InjectPolicies(IEnumerable<IDomainPolicy> policies);
}

public interface IAggregate<TState, TID> : IAggregate
    where TState : IState
    where TID : IID
{
    public IAggregate SetID(TID id);
    public TID ID { get; }

    // bool IsNew();
    // bool HasSourceId(IID sourceId);
    // void ApplyEvent(IEvent evt, long version);
    
}

public class Aggregate<TState, TID> : IAggregate<TState, TID>, IAggregate where TState : IState
    where TID : ID<TID>
{
    private object _mutex = new();

    private const long _newVersion = -1;

    public void InjectPolicies(IEnumerable<IDomainPolicy> aggregatePolicies)
    {
        foreach (var policy in aggregatePolicies)
        {
            policy.SetBehavior(this);
        }
    }

    public void ClearUncommittedEvents()
    {
        _uncommittedEvents.Clear();
    }

    public IEnumerable<IEvt> UncommittedEvents => _uncommittedEvents;

    private readonly ICollection<IEvt> _uncommittedEvents = new LinkedList<IEvt>();

    public IEnumerable<IEvt> AppliedEvents { get; }

    public string Type { get; set; }

    private bool _withAppliedEvents;

    protected TState _state;

    protected Aggregate(
        NewState<TState> newState,
        ITopicPubSub pubSub)
    {
        _state = newState();
        _pubSub = pubSub;
        Version = _newVersion;
    }

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
        _state = events.Aggregate(_state, (state, evt) => ApplyEvent(state, evt, Version++));
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

    private async Task RaiseEvent(IEvt evt)
    {
        ApplyEvent(_state, evt, Version++);
        _uncommittedEvents.Add(evt);
        await _pubSub.PublishAsync(evt.Topic, evt);
    }

    private IEnumerable<IDomainPolicy> _policies = Array.Empty<IDomainPolicy>();

    private readonly ITopicPubSub _pubSub;


    public async Task<IFeedback> ExecuteAsync(ICmd cmd)
    {
        var fbk = Feedback.New(cmd.GetID());
        try
        {
            fbk = ((dynamic)this).Verify((dynamic)cmd);
            
            if (!fbk.IsSuccess) return fbk;
            
            IEnumerable<IEvt> events = ((dynamic)this).Exec((dynamic)cmd);
            
            foreach (var @event in events)
            {
                await RaiseEvent(@event);
            }
            
            fbk.SetPayload(_state);
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsError());
        }

        return fbk;
    }

    private TState ApplyEvent(TState state, IEvt evt, long version)
    {
        if (_uncommittedEvents.Any(x => Equals(x.EventId, evt.EventId))) return _state;
        Version = version;
        return ((dynamic)this).Apply(state, (dynamic)evt);
    }
}

