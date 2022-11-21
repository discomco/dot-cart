using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public abstract class ListenerT<TFact, TCmd> : ActorB, ISubscriber
    where TFact : IFact
    where TCmd : ICmd
{
    private readonly IListenerDriverT<TFact> _driver;
    private readonly ICmdHandler _cmdHandler;
    private readonly Fact2Cmd<TCmd, TFact> _fact2Cmd;

    protected ListenerT(
        IListenerDriverT<TFact> driver,
        IExchange exchange,
        ICmdHandler cmdHandler,
        Fact2Cmd<TCmd, TFact> fact2Cmd) : base(exchange)
    {
        _driver = driver;
        _cmdHandler = cmdHandler;
        _fact2Cmd = fact2Cmd;
    }

    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken=default)
    {
        var fact = (TFact)msg;
        var cmd = _fact2Cmd(fact);
        return _cmdHandler.HandleAsync(cmd, cancellationToken);
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>)Task.CompletedTask;
    }

}