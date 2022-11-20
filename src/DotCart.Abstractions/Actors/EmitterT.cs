using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using static System.Threading.Tasks.Task;


namespace DotCart.Abstractions.Actors;

public interface IEmitterB : IActor
{
}

public abstract class EmitterT<TEmitterDriver, TEvt, TFact> : ActorB, IEmitterB
    where TEmitterDriver : IEmitterDriver
    where TEvt : IEvt
    where TFact : IFact
{
    private readonly TEmitterDriver _emitterDriver;
    private readonly Evt2Fact<TFact, TEvt> _evt2Fact;


    protected EmitterT(
        IExchange exchange,
        TEmitterDriver emitterDriver,
        Evt2Fact<TFact, TEvt> evt2Fact) : base(exchange)
    {
        _evt2Fact = evt2Fact;
        _emitterDriver = emitterDriver;
    }


    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken)
    {
        return Handler((IEvt)msg, cancellationToken);
    }


    private Task Handler(IEvt evt, CancellationToken cancellationToken)
    {
        return Run(() =>
        {
            var fact = _evt2Fact((Event)evt);
            return _emitterDriver.EmitFactAsync(fact);
        }, cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken)
    {
        return Run(() =>
        {
            _exchange.Unsubscribe(TopicAtt.Get<TEvt>(), this);
            return CompletedTask;
        }, cancellationToken);
    }
}