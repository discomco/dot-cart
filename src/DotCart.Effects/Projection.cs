using DotCart.Behavior;
using DotCart.Schema;
using Microsoft.Extensions.Hosting;
using Serilog;
using static System.Threading.Tasks.Task;

namespace DotCart.Effects;

public abstract class Projection<TState, TEvt> : BackgroundService, IProjection<TState, TEvt>
    where TState : IState
    where TEvt : IEvt
{
    private readonly ITopicMediator _mediator;
    private readonly IStore<TState> _store;

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
            await _store.SetAsync(state).ConfigureAwait(false);
        });
    }

    protected abstract TState Project(TState state, IEvt evt);
}