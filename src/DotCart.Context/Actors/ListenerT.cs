using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Context.Actors;

public abstract class ListenerT<TCmdPayload, TMeta, TFactPayload>
    : ActorB, ISubscriber
    where TCmdPayload : IPayload 
    where TMeta : IEventMeta
    where TFactPayload : IPayload
{
    private readonly ICmdHandler _cmdHandler;
    private readonly Fact2Cmd<TCmdPayload,  TMeta, TFactPayload> _fact2Cmd;

    protected ListenerT(
        IListenerDriverB driver,
        IExchange exchange,
        ICmdHandler cmdHandler,
        Fact2Cmd<TCmdPayload, TMeta, TFactPayload> fact2Cmd) : base(exchange)
    {
        Driver = driver;
        _cmdHandler = cmdHandler;
        _fact2Cmd = fact2Cmd;
    }

    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken = default)
    {
        var fact = (FactT<TFactPayload,TMeta>)msg;
        var cmd = _fact2Cmd(fact);
        return _cmdHandler.HandleAsync(cmd, cancellationToken);
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>)Task.CompletedTask;
    }
}