using DotCart.Context.Behaviors;
using DotCart.Context.Effects.Drivers;
using DotCart.Contract.Dtos;
using DotCart.Contract.Schemas;
using DotCart.Core;
using Serilog;
using static System.Threading.Tasks.Task;

namespace DotCart.Context.Effects;

public delegate TState Evt2State<TState, TEvt>(TState state, Event evt) where TState : IState where TEvt : IEvt;

public interface IProjection<TDriver, TState, in TEvt> : IReactor
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
public abstract class Projection<TSpoke, TDriver, TState, TEvt> : Reactor<TSpoke>, IProjection<TDriver, TState, TEvt>
    where TDriver : IModelStore<TState>
    where TState : IState
    where TEvt : IEvt
    where TSpoke : ISpoke<TSpoke>
{
    private readonly Evt2State<TState, TEvt> _evt2State;
    private readonly ITopicMediator _mediator;
    private readonly IModelStore<TState> _modelStore;

    protected Projection(
        ITopicMediator mediator,
        IModelStore<TState> modelStore,
        Evt2State<TState, TEvt> evt2State)
    {
        _mediator = mediator;
        _modelStore = modelStore;
        _evt2State = evt2State;
    }

    public override Task HandleAsync(IMsg msg, CancellationToken cancellationToken)
    {
        return CompletedTask;
    }

    private Task Handler(IEvt evt)
    {
        return Run(async () =>
        {
            Log.Information($"[{GetType().Name}] ~> [{_modelStore.GetType().Name}] ");
            var state = await _modelStore.GetByIdAsync(evt.AggregateID.Id()).ConfigureAwait(false);
            state = _evt2State(state, (Event)evt);
            await _modelStore.SetAsync(evt.AggregateID.Id(), state).ConfigureAwait(false);
        });
    }

    protected override Task StartReactingAsync(CancellationToken cancellationToken)
    {
        return Run(() =>
        {
            try
            {
                _mediator.SubscribeAsync(Topic.Get<TEvt>(), Handler);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }, cancellationToken);
    }

    protected override Task StopReactingAsync(CancellationToken cancellationToken)
    {
        return _mediator.UnsubscribeAsync(Topic.Get<TEvt>(), Handler);
    }
}