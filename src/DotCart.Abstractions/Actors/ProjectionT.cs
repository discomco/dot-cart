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
/// <typeparam name="TStore">
///     The Type of the Driver this Projection uses.
///     This type depends on the supporting backing service to which the Projection Projects its State.
/// </typeparam>
/// <typeparam name="TDoc">The type of the document that is being projected to.</typeparam>
/// <typeparam name="TEvt">The type of the Event that is being projected</typeparam>
public abstract class ProjectionT<TStore, TDoc, TEvt> : ActorB, IProjectionT<TStore, TDoc, TEvt>
    where TStore : IModelStore<TDoc>
    where TDoc : IState
    where TEvt : IEvtB
{
    private readonly Evt2State<TDoc, TEvt> _evt2Doc;
    private readonly TStore _modelStore;
    private readonly StateCtorT<TDoc> _newDoc;

    protected ProjectionT(
        IExchange exchange,
        TStore modelStore,
        Evt2State<TDoc, TEvt> evt2Doc,
        StateCtorT<TDoc> newDoc) : base(exchange)
    {
        _modelStore = modelStore;
        _evt2Doc = evt2Doc;
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
        if (!evt.IsCommitted) return;
        Log.Information($"{AppVerbs.Projecting} [{evt.AggregateId}::{evt.Topic}] ~> [{GetType().Name}]");
        var docId = GetDocId(evt);
        var doc = await _modelStore.GetByIdAsync(docId, cancellationToken).ConfigureAwait(false)
                  ?? _newDoc();
        doc = _evt2Doc(doc, (Event)evt);
        await _modelStore.SetAsync(docId, doc, cancellationToken).ConfigureAwait(false);
    }

    private string GetDocId(IEvtB evt)
    {
        try
        {
            return DocIdAtt.Get(this);
        }
        catch (Exception)
        {
            return evt.AggregateId;    
        }
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken)
    {
        return Run(() =>
        {
            Log.Information($"{AppFacts.Subscribed} {TopicAtt.Get<TEvt>()}  ~> [{GetType().Name}]");
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