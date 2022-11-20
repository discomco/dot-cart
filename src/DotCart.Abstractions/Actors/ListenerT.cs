using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public abstract class ListenerT<TListenerDriver, TFact, TCmd> : ActorB, IListener
    where TListenerDriver : IListenerDriver
    where TFact : IFact
    where TCmd : ICmd
{
    private readonly ICmdHandler _cmdHandler;
    private readonly TListenerDriver _driver;
    private readonly Fact2Cmd<TCmd, TFact> _fact2Cmd;

    protected ListenerT(
        IExchange exchange,
        ICmdHandler cmdHandler,
        TListenerDriver driver,
        Fact2Cmd<TCmd, TFact> fact2Cmd) : base(exchange)
    {
        _cmdHandler = cmdHandler;
        _driver = driver;
        _fact2Cmd = fact2Cmd;
    }

    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken)
    {
        var fact = (TFact)msg;
        var cmd = _fact2Cmd(fact);
        return _cmdHandler.HandleAsync(cmd, cancellationToken);
    }


    protected override Task StartActingAsync(CancellationToken cancellationToken)
    {
        return _driver.StartListening<TFact>(cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken)
    {
        return _driver.StopListening<TFact>(cancellationToken);
    }
}