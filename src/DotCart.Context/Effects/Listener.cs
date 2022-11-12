using DotCart.Context.Behaviors;
using DotCart.Context.Effects.Drivers;
using DotCart.Contract.Dtos;

namespace DotCart.Context.Effects;

public delegate TCmd Fact2Cmd<in TFact, out TCmd>(TFact fact)
    where TFact : IFact
    where TCmd : ICmd;

public interface IListener : IReactor
{
}

public abstract class Listener<TSpoke, TDriver, TFact, TCmd> : Reactor<TSpoke>, IListener
    where TDriver : IListenerDriver
    where TFact : IFact
    where TCmd : ICmd
    where TSpoke : ISpoke<TSpoke>
{
    private readonly ICmdHandler _cmdHandler;
    private readonly TDriver _driver;
    private readonly Fact2Cmd<TFact, TCmd> _fact2Cmd;

    protected Listener(
        ICmdHandler cmdHandler,
        TDriver driver,
        Fact2Cmd<TFact, TCmd> fact2Cmd)
    {
        _cmdHandler = cmdHandler;
        _driver = driver;
        _fact2Cmd = fact2Cmd;
    }

    public override Task HandleAsync(IMsg msg, CancellationToken cancellationToken)
    {
        var fact = (TFact)msg;
        var cmd = _fact2Cmd(fact);
        return _cmdHandler.HandleAsync(cmd);
    }


    protected override Task StartReactingAsync(CancellationToken cancellationToken)
    {
        return _driver.StartListening<TFact>(cancellationToken);
    }

    protected override Task StopReactingAsync(CancellationToken cancellationToken)
    {
        return _driver.StopListening<TFact>(cancellationToken);
    }
}