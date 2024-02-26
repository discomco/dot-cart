using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;
using DotCart.Actors;
using DotCart.Core;
using Serilog;
using NameAtt = DotCart.Abstractions.NameAtt;

namespace DotCart.Behavior;

public class AggregatePolicyT<TEvtPayload, TCmdPayload, TMeta>
    : ActorB, IAggregatePolicy
    where TCmdPayload : IPayload
    where TMeta : IMetaB
{
    private readonly Evt2Cmd<TCmdPayload, TEvtPayload, TMeta> _evt2Cmd;

    private IAggregate? Aggregate;

    protected AggregatePolicyT(
        IExchange exchange,
        Evt2Cmd<TCmdPayload, TEvtPayload, TMeta> evt2Cmd
    ) : base(exchange)
    {
        _evt2Cmd = evt2Cmd;
        exchange.Subscribe(EvtTopicAtt.Get<TEvtPayload>(), this);
    }


    public void SetAggregate(IAggregate aggregate)
    {
        Aggregate = aggregate;
    }

    public string Topic => EvtTopicAtt.Get<TEvtPayload>();

    public async Task HandleEvtAsync(IEvtB evt, CancellationToken cancellationToken = default)
    {
        if (evt.IsCommitted)
            return;
        var fbk = await this
            .EnforceAsync((dynamic)evt, cancellationToken)
            .ConfigureAwait(false);
        if (!fbk.IsSuccess)
            Log.Error($"{AppErrors.Error($"[{GetType()} on {evt.Topic}] Failed => {fbk.ErrState}")}");
    }

    private async Task<IFeedback> EnforceAsync(
        IEvtB evt,
        CancellationToken cancellationToken = default)
    {
        var feedback = Feedback.New(evt.AggregateId);
        try
        {
            Log.Information($"{AppVerbs.Enforcing} [{NameAtt.Get(this)}] on {evt.Topic}");
            Aggregate.SetID(evt.AggregateId.IDFromIdString());
            var cmd = _evt2Cmd((Event)evt, Aggregate.GetState());
            if (cmd != null)
                feedback = await Aggregate.ExecuteAsync(cmd);
            Log.Information($"{AppFacts.Enforced} [{NameAtt.Get(this)}] on {evt.Topic}");
        }
        catch (Exception e)
        {
            feedback.SetError(e.AsError());
            Log.Error($"{AppErrors.Error(e.InnerAndOuter())}");
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