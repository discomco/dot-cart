using DotCart.Behavior;
using DotCart.Schema;
using Microsoft.Extensions.Hosting;
using Serilog;
using static System.Threading.Tasks.Task;

namespace DotCart.Effects;

/// <summary>
///     A Projection is an active Unit of Effect (BackgroundService) that is defined
///     by the TState that it projects the TEvt to.
/// </summary>
/// <typeparam name="TState"></typeparam>
/// <typeparam name="TEvt"></typeparam>
public abstract class Projection<TState, TEvt> : BackgroundService, IProjection<TState, TEvt>
    where TState : IState
    where TEvt : IEvt
{
    private readonly ITopicMediator _mediator;
    private readonly IStore<TState> _store;
    private ISpoke _spoke;

    protected Projection(
        ITopicMediator mediator,
        IStore<TState> store
    )
    {
        _mediator = mediator;
        _store = store;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
            }
        }, stoppingToken).ConfigureAwait(false);
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return StartProjecting(cancellationToken);
    }

    private Task StartProjecting(CancellationToken cancellationToken)
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


    private Task Handler(IEvt evt)
    {
        return Run(async () =>
        {
            var state = await _store.GetByIdAsync(evt.AggregateId).ConfigureAwait(false);
            state = Project(state, evt);
            await _store.SetAsync(evt.AggregateId, state).ConfigureAwait(false);
        });
    }

    protected abstract TState Project(TState state, IEvt evt);
 
    public void SetSpoke(ISpoke spoke)
    {
        _spoke = spoke;
    }
}