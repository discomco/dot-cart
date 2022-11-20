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

public interface IProjectionT3<TDriver, TState, in TEvt> : IProjectionB
    where TDriver : IModelStore<TState>
    where TState : IState
    where TEvt : IEvt
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
/// <typeparam name="TState">The type of the document that is being projected to.</typeparam>
/// <typeparam name="TEvt">The type of the Event that is being projected</typeparam>
public abstract class ProjectionT<TDriver, TState, TEvt> : ActorB, IProjectionT3<TDriver, TState, TEvt>
    where TDriver : IModelStore<TState>
    where TState : IState
    where TEvt : IEvt
{
    private readonly Evt2State<TState, TEvt> _evt2State;
    private readonly TDriver _modelStore;

    private readonly object _subMutex = new();

    private readonly object _unsubMutex = new();

    protected ProjectionT(
        IExchange exchange,
        TDriver modelStore,
        Evt2State<TState, TEvt> evt2State) : base(exchange)
    {
        _modelStore = modelStore;
        _evt2State = evt2State;
    }

    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken)
    {
        return Handler((IEvt)msg, cancellationToken);
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>)CompletedTask;
    }

    private async Task Handler(IEvt evt, CancellationToken cancellationToken = default)
    {
        Log.Information($"PROJECTION::[{GetType().Name}] ~> [{evt.Topic}] => [{evt.AggregateID.Id()}] ");
        var state = await _modelStore.GetByIdAsync(evt.AggregateID.Id(), cancellationToken).ConfigureAwait(false);
        state = _evt2State(state, (Event)evt);
        await _modelStore.SetAsync(evt.AggregateID.Id(), state, cancellationToken).ConfigureAwait(false);
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken)
    {
        return Run(() =>
        {
            Log.Information($"[{GetType().Name}] subscribed to {TopicAtt.Get<TEvt>()} ");
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