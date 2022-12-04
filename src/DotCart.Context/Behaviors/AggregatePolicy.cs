using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Serilog;

namespace DotCart.Context.Behaviors;

public class AggregatePolicy<TEvt, TCmd> : ActorB, IAggregatePolicy where TEvt : IEvt where TCmd : ICmd
{
    private readonly Evt2Cmd<TCmd, TEvt> _evt2Cmd;
    protected IAggregate? Aggregate;

    protected AggregatePolicy
    (
        IExchange exchange,
        Evt2Cmd<TCmd, TEvt> evt2Cmd
    ) : base(exchange)
    {
        _evt2Cmd = evt2Cmd;
        exchange.Subscribe(TopicAtt.Get<TEvt>(), this);
    }


    public void SetBehavior(IAggregate aggregate)
    {
        Aggregate = aggregate;
    }

    private Task HandleEvtAsync(IEvt arg, CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            var fbk = await EnforceAsync((IEvt)arg);
            if (!fbk.IsSuccess)
                Log.Error($"[{GetType().Name}] Failed => {fbk.ErrState}");
            return Task.CompletedTask;
        }, cancellationToken);
    }

    private Task<Feedback> EnforceAsync(IEvt evt)
    {
        var cmd = _evt2Cmd((TEvt)evt);
        Aggregate.SetID(evt.AggregateId.IDFromIdString());
        return Aggregate.ExecuteAsync(cmd);
    }

    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken = default)
    {
        return HandleEvtAsync((IEvt)msg, cancellationToken);
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>)Task.CompletedTask;
    }
}