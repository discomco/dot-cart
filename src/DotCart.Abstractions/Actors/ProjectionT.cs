using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Serilog;
using static System.Threading.Tasks.Task;

namespace DotCart.Abstractions.Actors;

public interface IProjectionB : IActor
{
}

public interface IProjectionT<TDriver, TState, in TEvt> : IProjectionB
    where TDriver : IModelStore<TState>
    where TState : IState
    where TEvt : IEvtB
{
}

/// <summary>
///     A Projection is an active Unit of Effect (Reactor) that is defined
///     by the TState that it projects the TEvt to.
/// </summary>
/// <typeparam name="TDriver">
///     The Type of the Driver this Projection uses.
///     This type depends on the supporting backing service to which the Projection Projects its State.
/// </typeparam>
/// <typeparam name="TDoc">The type of the document that is being projected to.</typeparam>
/// <typeparam name="TEvt">The type of the Event that is being projected</typeparam>
public abstract class ProjectionT<TDriver, TDoc, TEvt> : ActorB, IProjectionT<TDriver, TDoc, TEvt>
    where TDriver : IModelStore<TDoc>
    where TDoc : IState
    where TEvt : IEvtB
{
    private readonly Evt2State<TDoc, TEvt> _evt2State;
    private readonly TDriver _modelStore;
    private readonly StateCtorT<TDoc> _newDoc;

    private readonly object _subMutex = new();

    private readonly object _unsubMutex = new();

    protected ProjectionT(
        IExchange exchange,
        TDriver modelStore,
        Evt2State<TDoc, TEvt> evt2State,
        StateCtorT<TDoc> newDoc) : base(exchange)
    {
        _modelStore = modelStore;
        _evt2State = evt2State;
        _newDoc = newDoc;
    }

    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken)
    {
        return Handler((IEvtB)msg, cancellationToken);
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>)CompletedTask;
    }

    private async Task Handler(IEvtB evt, CancellationToken cancellationToken = default)
    {
        Log.Information($"PROJECTION::[{GetType().Name}] ~> [{evt.Topic}] => [{evt.AggregateId}] ");
        var doc = await _modelStore.GetByIdAsync(evt.AggregateId, cancellationToken).ConfigureAwait(false)
                  ?? _newDoc();
        doc = _evt2State(doc, (Event)evt);
        await _modelStore.SetAsync(evt.AggregateId, doc, cancellationToken).ConfigureAwait(false);
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken)
    {
        return Run(() =>
        {
            var subscribed = AppFacts.Subscribed;
            Log.Information($"{subscribed} {TopicAtt.Get<TEvt>()}  ~> [{GetType().Name}]");
            _exchange.Subscribe(TopicAtt.Get<TEvt>(), this);
            return CompletedTask;
        }, cancellationToken);
    }

    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return Run(() =>
        {
            _exchange.Unsubscribe(TopicAtt.Get<TEvt>(), this);
            return CompletedTask;
        }, cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken)
    {
        return CompletedTask;
    }
}