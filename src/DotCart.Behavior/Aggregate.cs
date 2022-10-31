using Ardalis.GuardClauses;
using DotCart.Contract;
using DotCart.Schema;
using Serilog;

namespace DotCart.Behavior;

public delegate IAggregate AggCtor();

public delegate IAggregate AggBuilder(AggCtor newAgg);

public interface IAggregate
{
    IID ID { get; }
    bool IsNew { get; }
    long Version { get; }
    IEnumerable<IEvt> UncommittedEvents { get; }
    string Id();
    Task<IFeedback> ExecuteAsync(ICmd cmd);
    IState GetState();
    IAggregate SetID(IID ID);
    void Load(IEnumerable<IEvt>? events);
    void ClearUncommittedEvents();
    void InjectPolicies(IEnumerable<IDomainPolicy> policies);
    string GetName();
}

public abstract class Aggregate<TState, TID> : IAggregate
    where TState : IState
    where TID : ID<TID>
{
    private const long _newVersion = -1;
    private readonly ICollection<IEvt> _uncommittedEvents = new LinkedList<IEvt>();

    private readonly object execMutex = new();
    public ICollection<IEvt> _appliedEvents = new LinkedList<IEvt>();
    protected TState _state;
    private bool _withAppliedEvents = false;

    protected Aggregate(
        NewState<TState> newState)
    {
        _state = newState();
        Version = _newVersion;
    }

    public void InjectPolicies(IEnumerable<IDomainPolicy> aggregatePolicies)
    {
        foreach (var policy in aggregatePolicies) policy.SetBehavior(this);
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
            _state = events.Aggregate(_state, (state, evt) => ApplyEvent(state, evt, ++Version));
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
        return ID.Value;
    }

    public async Task<IFeedback> ExecuteAsync(ICmd cmd)
    {
        var feedback = Feedback.New(cmd.GetID());
        try
        {
            Guard.Against.BehaviorIDNotSet(this);
            feedback = ((dynamic)this).Verify((dynamic)cmd);
            if (!feedback.IsSuccess) return feedback;
            IEnumerable<IEvt> events = ((dynamic)this).Raise((dynamic)cmd);
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
        if (Version >= evt.Version) return;
        evt.SetTimeStamp(DateTime.UtcNow);
        _state = ApplyEvent(_state, evt, ++Version);
        _uncommittedEvents.Add(evt);
        //await _mediator.PublishAsync(evt.MsgType, evt);
    }


    private TState ApplyEvent(TState state, IEvt evt, long version)
    {
        if (_uncommittedEvents.Any(x => Equals(x.MsgId, evt.MsgId)))
            return _state;
        Version = version;
        evt.SetVersion(Version);
        evt.SetBehaviorType(GetName());
        _appliedEvents.Add(evt);
        return ((dynamic)this).Apply(state, (dynamic)evt);
    }
}