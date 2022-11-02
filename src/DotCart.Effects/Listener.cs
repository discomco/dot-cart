using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Effects.Drivers;

namespace DotCart.Effects;

public delegate TCmd Fact2Cmd<in TFact, out TCmd>(TFact fact) 
    where TFact: IFact 
    where TCmd: ICmd;

public interface IListener: IReactor
{
}

public abstract class Listener<TDriver, TFact, TCmd>: Reactor, IListener 
    where TDriver: IListenerDriver 
    where TFact : IFact 
    where TCmd : ICmd
{
    private readonly ICmdHandler _cmdHandler;
    private readonly TDriver _driver;
    private readonly Fact2Cmd<TFact, TCmd> _fact2Cmd;

    protected Listener(
        ICmdHandler cmdHandler,
        TDriver driver,
        Fact2Cmd<TFact,TCmd> fact2Cmd)
    {
        _cmdHandler = cmdHandler;
        _driver = driver;
        _fact2Cmd = fact2Cmd;
    }



    protected override Task StartReactingAsync(CancellationToken cancellationToken)
    {
        return _driver.StartListening<TFact>(cancellationToken);
    }

    protected override Task StopReactingAsync(CancellationToken cancellationToken)
    {
        return _driver.StopListening<TFact>(cancellationToken);
    }

    public override Task HandleAsync(IMsg msg)
    {
        var fact = (TFact)msg;
        var cmd = _fact2Cmd(fact);
        return _cmdHandler.Handle(cmd);
    }
    
}