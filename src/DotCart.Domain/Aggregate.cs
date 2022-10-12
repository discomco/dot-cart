using System.Dynamic;
using DotCart.Contract;
using DotCart.Schema;

namespace DotCart.Domain;

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
}

public interface IAggregate<TState, TID> : IAggregate
    where TState : IState
    where TID : IID
{
    public IAggregate SetID(TID id);
    public TID ID { get; }

    public string Name { get; }
    void InjectPolicies(IEnumerable<IAggregatePolicy> aggregatePolicies);

    // bool IsNew();
    // bool HasSourceId(IID sourceId);
    // void ApplyEvent(IEvent evt, long version);
    // void ClearUncommittedEvents();
    // void Load(IEnumerable<IEvent> events);
    IEnumerable<IEvt> UncommittedEvents { get; }
}

public class Aggregate<TState, TID> : IAggregate<TState, TID>, IAggregate where TState : IState
    where TID : ID<TID>
{
    private object _mutex = new();

    private const long _newVersion = -1;

    public void InjectPolicies(IEnumerable<IAggregatePolicy> aggregatePolicies)
    {
        foreach (var aggregatePolicy in aggregatePolicies)
        {
            aggregatePolicy.SetAggregate(this);
        }
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
        ApplyEvent(evt, Version++);
        _uncommittedEvents.Add(evt);
        await _pubSub.PublishAsync(evt.Topic, evt);
    }

    private IEnumerable<IAggregatePolicy> _policies = Array.Empty<IAggregatePolicy>();

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
            fbk.SetError(e.AsApiError());
        }

        return fbk;
    }

    private void ApplyEvent(IEvt evt, long version)
    {
        if (_uncommittedEvents.Any(x => Equals(x.EventId, evt.EventId))) return;
        ((dynamic)this).Apply((dynamic)evt);
        Version = version;
    }
}

public abstract class AggregateBase
{
}