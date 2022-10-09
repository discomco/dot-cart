using System.Collections.Immutable;
using DotCart.Schema;

namespace DotCart.Domain;

public interface ICmd
{}

public delegate IEnumerable<IEvent> TryCmd(IState state, ICmd cmd);
public delegate IState ApplyEvt(IState state, IEvent evt);


public delegate IAggregate AggCtor();

public delegate IAggregate AggBuilder(AggCtor newAgg);

public interface IAggregate
{
    // public IID ID { get; }
    // public IID GetName();
    // long GetVersion();
    // bool IsNew();
    // bool HasSourceId(IID sourceId);
    // void ApplyEvent(IEvent evt, long version);
    // void ClearUncommittedEvents();
    // void Load(IEnumerable<IEvent> events);
    // IEnumerable<IEvent> GetUncommittedEvents();

}


public class Aggregate<TState> : IAggregate
    where TState : IState
{
    private const long _newVersion = -1;
    public IEnumerable<IEvent> UncommittedEvents { get;  }
    private readonly ICollection<IEvent> _uncommitedEvents = new LinkedList<IEvent>();

    public IEnumerable<IEvent> AppliedEvents { get;  }
    

    
    public string Type { get; set; }
    
    private bool _withAppliedEvents;
    
    private IImmutableDictionary<string, TryCmd> _executors;
    
    private IImmutableDictionary<string, ApplyEvt> _appliers;
    
    private TState _state;

    public Aggregate(
        NewState<TState> newState,
        IImmutableDictionary<string, TryCmd> executors, 
        IImmutableDictionary<string, ApplyEvt> appliers)
    {
        _appliers = appliers;
        _executors = executors;
        _state = newState();
    }
    
}