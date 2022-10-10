using System.Collections.Immutable;
using DotCart.Contract;
using DotCart.Schema;

namespace DotCart.Domain;

public delegate IAggregate AggCtor();
public delegate IAggregate AggBuilder(AggCtor newAgg);


public interface IAggregate
{
    bool IsNew { get; }
    string Name { get;  }
    string Id();
    long Version { get; }
}

public interface IAggregate<TState, TID> 
    where TState:IState 
    where TID: IID
{
    public TState GetState();
    public IAggregate SetID(TID id);
    public TID ID { get; }
    public string Name { get; }
    // bool IsNew();
    // bool HasSourceId(IID sourceId);
    // void ApplyEvent(IEvent evt, long version);
    // void ClearUncommittedEvents();
    // void Load(IEnumerable<IEvent> events);
    IEnumerable<IEvt> UncommittedEvents { get; }
    IFeedback Execute(TState state, ICmd cmd);

}


public class Aggregate<TState, TID> : IAggregate<TState, TID>, IAggregate where TState : IState
    where TID: ID<TID>
{
    private const long _newVersion = -1;

    public IEnumerable<IEvt> UncommittedEvents => _uncommittedEvents;

    private readonly ICollection<IEvt> _uncommittedEvents = new LinkedList<IEvt>();

    public IEnumerable<IEvt> AppliedEvents { get;  }
    
    public string Type { get; set; }
    
    private bool _withAppliedEvents;
    
    private readonly TState _state;

    public Aggregate(
        NewState<TState> newState)
    {
        _state = newState();
        Version = _newVersion;
    }
    public TState GetState()
    {
        return _state;
    }

    public TID ID { get; private set; }

    public bool IsNew  => Version == -1;
    public long Version { get; set; }
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

    private void RaiseEvent(IEvt evt)
    {
        ApplyEvent(evt, Version++);
        _uncommittedEvents.Add(evt);
    }
    public IFeedback Execute(TState state, ICmd cmd)
    {
        var fbk = new Feedback(cmd.GetID().Value, Array.Empty<byte>());
        try
        {
            fbk = ((dynamic)this).Verify(state, (dynamic)cmd);
            if (!fbk.IsSuccess) return fbk;
            IEnumerable<IEvt> events = ((dynamic)this).Exec(state, (dynamic)cmd);
            foreach (var @event in events)
            {
                RaiseEvent(@event);
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
        ((dynamic)this).Apply(_state, (dynamic)evt);
        Version = version;
    }

    
    
}