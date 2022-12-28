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
    where TDriver : IDocStore<TState>
    where TState : IState
    where TEvt : IEvtB
{
}

/// <summary>
///     A Projection is an active Unit of Effect (Actor) that is defined
///     by the TStore and the TDoc that it projects the TEvt to.
/// </summary>
/// <typeparam name="TIStore">
///     The Type of the Store this Actor projects to
///     This type depends on the supporting backing service to which the Projection Projects its State.
/// </typeparam>
/// <typeparam name="TDoc">The type of the document that is being projected to.</typeparam>
/// <typeparam name="TIEvt">The type of the Event that is being projected</typeparam>
public abstract class ProjectionT<TIStore, TDoc, TIEvt>
    : ActorB, IProjectionT<TIStore, TDoc, TIEvt>
    where TIStore : IDocStore<TDoc>
    where TDoc : IState
    where TIEvt : IEvtB
{
    private readonly TIStore _docStore;
    private readonly Evt2Doc<TDoc, TIEvt> _evt2Doc;
    private readonly StateCtorT<TDoc> _newDoc;

    protected ProjectionT(
        IExchange exchange,
        TIStore docStore,
        Evt2Doc<TDoc, TIEvt> evt2Doc,
        StateCtorT<TDoc> newDoc) : base(exchange)
    {
        _docStore = docStore;
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
        var doc = await _docStore.GetByIdAsync(docId, cancellationToken).ConfigureAwait(false)
                  ?? _newDoc();
        doc = _evt2Doc(doc, (Event)evt);

        // TODO: Call ProjectionValidationFunc here

        await _docStore.SetAsync(docId, doc, cancellationToken).ConfigureAwait(false);
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
            Log.Information($"{AppFacts.Subscribed} {TopicAtt.Get<TIEvt>()}  ~> [{GetType().Name}]");
            _exchange.Subscribe(TopicAtt.Get<TIEvt>(), this);
            return CompletedTask;
        }, cancellationToken);
    }

    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return Run(() =>
        {
            _exchange.Unsubscribe(TopicAtt.Get<TIEvt>(), this);
            return CompletedTask;
        }, cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken)
    {
        return CompletedTask;
    }
}