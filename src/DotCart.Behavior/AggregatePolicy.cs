using DotCart.Contract;
using Serilog;

namespace DotCart.Behavior;


public interface IAggregatePolicy
{
    void SetBehavior(IAggregate aggregate);
}

public delegate TCmd Evt2Cmd<in TEvt, out TCmd>(Event Evt) where TEvt : IEvt where TCmd : ICmd;

public abstract class AggregatePolicy<TEvt, TCmd> : IAggregatePolicy where TEvt : IEvt where TCmd : ICmd
{
    private readonly ILogger _logger;
    private readonly Evt2Cmd<TEvt, TCmd> _evt2Cmd;
    protected IAggregate? Aggregate;

    protected AggregatePolicy
    (
        ILogger logger,
        ITopicMediator mediator,
        Evt2Cmd<TEvt, TCmd> evt2Cmd
    )
    {
        _logger = logger;
        _evt2Cmd = evt2Cmd;
        mediator.SubscribeAsync(Topic.Get<TEvt>(), HandleEvtAsync);
    }


    public void SetBehavior(IAggregate aggregate)
    {
        Aggregate = aggregate;
    }

    private Task HandleEvtAsync(IEvt arg)
    {
        return Task.Run(async () =>
        {
            var fbk = await EnforceAsync((Event)arg);
            if (!fbk.IsSuccess)
                _logger.Error($"[{GetType().Name}] Failed => {fbk.ErrState}");
        });
    }

    private Task<IFeedback> EnforceAsync(Event evt)
    {
        var cmd = _evt2Cmd(evt);
        Aggregate.SetID(evt.AggregateID);
        return Aggregate.ExecuteAsync(cmd);
    }
}