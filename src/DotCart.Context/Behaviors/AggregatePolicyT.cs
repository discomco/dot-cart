using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Serilog;

namespace DotCart.Context.Behaviors;

public class AggregatePolicyT<TEvt, TCmd> : ActorB, IAggregatePolicy
    where TEvt : IEvtB
    where TCmd : ICmdB
{
    private readonly Evt2Cmd<TCmd, TEvt> _evt2Cmd;

    private IAggregate? Aggregate;

    protected AggregatePolicyT
    (
        IExchange exchange,
        Evt2Cmd<TCmd, TEvt> evt2Cmd
    ) : base(exchange)
    {
        _evt2Cmd = evt2Cmd;
        exchange.Subscribe(TopicAtt.Get<TEvt>(), this);
    }


    public void SetAggregate(IAggregate aggregate)
    {
        Aggregate = aggregate;
    }

    public string Topic => TopicAtt.Get<TEvt>();

    public async Task HandleEvtAsync(IEvtB evt, CancellationToken cancellationToken = default)
    {
        if (evt.IsCommitted)
            return;
        var fbk = await EnforceAsync((dynamic)evt);
        if (!fbk.IsSuccess)
            Log.Error($"{AppErrors.Error} [{GetType()} on {evt.Topic}] Failed => {fbk.ErrState}");
    }

    private async Task<Feedback> EnforceAsync(IEvtB evt)
    {
        var feedback = Feedback.New(evt.AggregateId);
        try
        {
            Log.Information($"{AppVerbs.Enforcing} [{NameAtt.Get(this)}] on {evt.Topic}");
            Aggregate.SetID(evt.AggregateId.IDFromIdString());
            var cmd = _evt2Cmd((Event)evt, Aggregate.GetState());
            if (cmd != null)
                feedback = await Aggregate.ExecuteAsync(cmd);
        }
        catch (Exception e)
        {
            feedback.SetError(e.AsError());
        }

        return feedback;
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
        return HandleEvtAsync((IEvtB)msg, cancellationToken);
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>)Task.CompletedTask;
    }
}