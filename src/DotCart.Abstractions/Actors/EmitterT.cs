using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Serilog;
using static System.Threading.Tasks.Task;


namespace DotCart.Abstractions.Actors;

public interface IEmitterB : IActor
{
}

public abstract class EmitterT<TEvt, TFact> : ActorB, IEmitterB
    where TEvt : IEvt
    where TFact : IFact
{
    private readonly Evt2Fact<TFact, TEvt> _evt2Fact;


    protected EmitterT(
        IExchange exchange,
        Evt2Fact<TFact, TEvt> evt2Fact) : base(exchange)
    {
        _evt2Fact = evt2Fact;
    }


    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken)
    {
        return Run(() =>
        {
            var fact = _evt2Fact((TEvt)msg);
            return EmitFactAsync(fact);
        }, cancellationToken);
    }

    protected abstract Task EmitFactAsync(TFact fact);

    protected override Task StartActingAsync(CancellationToken cancellationToken = default)
    {
        return Run(() =>
        {
            Log.Information($":: EMITTER :: [{GetType()}] ~> STARTED");
            _exchange.Subscribe(TopicAtt.Get<TEvt>(), this);
            return CompletedTask;
        }, cancellationToken);
    }


    protected override Task StopActingAsync(CancellationToken cancellationToken = default)
    {
        return Run(() =>
        {
            Log.Information($":: EMITTER :: [{GetType()}] ~> STOPPED");
            _exchange.Unsubscribe(TopicAtt.Get<TEvt>(), this);
            return CompletedTask;
        }, cancellationToken);
    }
}