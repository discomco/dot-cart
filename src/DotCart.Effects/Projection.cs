using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Effects.Drivers;
using DotCart.Schema;
using Serilog;
using static System.Threading.Tasks.Task;

namespace DotCart.Effects;

public delegate TState Evt2State<TState, TEvt>(TState state, IEvt evt) where TState : IState where TEvt : IEvt;

/// <summary>
///     A Projection is an active Unit of Effect (BackgroundService) that is defined
///     by the TState that it projects the TEvt to.
/// </summary>
/// <typeparam name="TState"></typeparam>
/// <typeparam name="TEvt"></typeparam>
public class Projection<TDriver, TState, TEvt> : Reactor, IProjection<TState, TEvt>
    where TDriver : IProjectionDriver<TState>
    where TState : IState
    where TEvt : IEvt
{
    private readonly ITopicMediator _mediator;
    private readonly IProjectionDriver<TState> _projectionDriver;
    private readonly Evt2State<TState, TEvt> _evt2State;

    protected Projection(
        ITopicMediator mediator,
        IProjectionDriver<TState> projectionDriver,
        Evt2State<TState, TEvt> evt2State)
    {
        _mediator = mediator;
        _projectionDriver = projectionDriver;
        _evt2State = evt2State;
    }

    private Task Handler(IEvt evt)
    {
        return Run(async () =>
        {
            var state = await _projectionDriver.GetByIdAsync(evt.AggregateId).ConfigureAwait(false);
            state = _evt2State(state, evt);
            await _projectionDriver.SetAsync(evt.AggregateId, state).ConfigureAwait(false);
        });
    }

    protected override Task StartReactingAsync(CancellationToken cancellationToken)
    {
        return Run(() =>
        {
            try
            {
                _mediator.Subscribe(Topic.Get<TEvt>(), Handler);
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

    public override Task HandleAsync(IMsg msg)
    {
        throw new NotImplementedException();
    }
}