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

public abstract class EmitterT<TPayload, TMeta>
    : ActorB, IEmitterB
    where TPayload : IPayload
    where TMeta : IEventMeta
{
    private readonly Evt2Fact<TPayload, TMeta> _evt2Fact;


    protected EmitterT(
        IEmitterDriverT<TPayload, TMeta> driver,
        IExchange exchange,
        Evt2Fact<TPayload, TMeta> evt2Fact) : base(exchange)
    {
        Driver = driver;
        _evt2Fact = evt2Fact;
    }


    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken)
    {
        return Run(() =>
        {
            var fact = _evt2Fact((EvtT<TPayload, TMeta>)msg);
            return EmitFactAsync(fact);
        }, cancellationToken);
    }

    protected abstract Task EmitFactAsync(FactT<TPayload, TMeta> fact);

    protected override Task StartActingAsync(CancellationToken cancellationToken = default)
    {
        return Run(() =>
        {
            Log.Information($"{AppFacts.Started} {Name}");
            _exchange.Subscribe(EvtTopicAtt.Get<TPayload>(), this);
            return CompletedTask;
        }, cancellationToken);
    }


    protected override Task StopActingAsync(CancellationToken cancellationToken = default)
    {
        return Run(() =>
        {
            Log.Information($"{AppFacts.Stopped} {Name}");
            _exchange.Unsubscribe(EvtTopicAtt.Get<TPayload>(), this);
            return CompletedTask;
        }, cancellationToken);
    }
}