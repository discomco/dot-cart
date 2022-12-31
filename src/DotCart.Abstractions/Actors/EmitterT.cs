using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Serilog;
using static System.Threading.Tasks.Task;


namespace DotCart.Abstractions.Actors;

public interface IEmitterB : IActor
{
}

public abstract class EmitterT<TIEvt, TIFact, TPayload>
    : ActorB, IEmitterB
    where TIEvt : IEvtB
    where TIFact : IFactB
    where TPayload : IPayload
{
    private readonly Evt2Fact<TIFact, TIEvt> _evt2Fact;


    protected EmitterT(
        IEmitterDriverT<TPayload> driver,
        IExchange exchange,
        Evt2Fact<TIFact, TIEvt> evt2Fact) : base(exchange)
    {
        Driver = driver;
        _evt2Fact = evt2Fact;
    }


    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken)
    {
        return Run(() =>
        {
            var fact = _evt2Fact((Event)msg);
            return EmitFactAsync(fact);
        }, cancellationToken);
    }

    protected abstract Task EmitFactAsync(TIFact fact);

    protected override Task StartActingAsync(CancellationToken cancellationToken = default)
    {
        return Run(() =>
        {
            Log.Information($":: EMITTER :: [{GetType()}] ~> STARTED");
            _exchange.Subscribe(TopicAtt.Get<TIEvt>(), this);
            return CompletedTask;
        }, cancellationToken);
    }


    protected override Task StopActingAsync(CancellationToken cancellationToken = default)
    {
        return Run(() =>
        {
            Log.Information($":: EMITTER :: [{GetType()}] ~> STOPPED");
            _exchange.Unsubscribe(TopicAtt.Get<TIEvt>(), this);
            return CompletedTask;
        }, cancellationToken);
    }
}