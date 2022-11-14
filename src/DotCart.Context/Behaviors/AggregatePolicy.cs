using DotCart.Context.Abstractions;
using DotCart.Context.Effects;
using DotCart.Contract.Dtos;
using DotCart.Core;
using Serilog;

namespace DotCart.Context.Behaviors;

public delegate TCmd Evt2Cmd<in TEvt, out TCmd>(Event Evt) where TEvt : IEvt where TCmd : ICmd;

public class AggregatePolicy<TEvt, TCmd> : Actor, IAggregatePolicy where TEvt : IEvt where TCmd : ICmd
{
    private readonly Evt2Cmd<TEvt, TCmd> _evt2Cmd;
    protected IAggregate? Aggregate;

    protected AggregatePolicy
    (
        IExchange exchange,
        Evt2Cmd<TEvt, TCmd> evt2Cmd
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
            var fbk = await EnforceAsync((Event)arg);
            if (!fbk.IsSuccess)
                Log.Error($"[{GetType().Name}] Failed => {fbk.ErrState}");
            return Task.CompletedTask;
        }, cancellationToken);
    }

    private Task<IFeedback> EnforceAsync(Event evt)
    {
        var cmd = _evt2Cmd(evt);
        Aggregate.SetID(evt.AggregateID);
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