using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Behavior;
using DotCart.Core;
using DotCart.Schema;
using Serilog;
using static System.Threading.Tasks.Task;


namespace DotCart.Actors;

public interface IEmitterB : IActorB
{
}

public abstract class EmitterT<TSpoke, TPayload, TMeta>
    : ActorB, IActorT<TSpoke>, IEmitterT<TSpoke, TPayload, TMeta>
    where TPayload : IPayload
    where TMeta : IMetaB
    where TSpoke : ISpokeT<TSpoke>
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
        try
        {
            var fact = _evt2Fact((Event)msg);
            Log.Information($"{AppVerbs.Emitting} [{fact.Topic}] ~> [{Driver.GetType().Name}]");
            return ((IEmitterDriverT<TPayload, TMeta>)Driver).EmitAsync(fact, cancellationToken);
        }
        catch (Exception e)
        {
            Log.Error($"{AppErrors.Error(e.InnerAndOuter())}");
            return CompletedTask;
        }
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>)CompletedTask;
    }

    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return CompletedTask;
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken = default)
    {
        return Run(async () =>
        {
            await ((IEmitterDriverB)Driver).ConnectAsync();
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