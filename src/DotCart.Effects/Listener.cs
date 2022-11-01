using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Effects.Drivers;

namespace DotCart.Effects;

public delegate TCmd Fact2Cmd<in TFact, out TCmd>(TFact fact) 
    where TFact: IFact 
    where TCmd: ICmd;


public abstract class Listener<TDriver, TFact, TCmd>: Reactor, IListener 
    where TDriver: IListenerDriver 
    where TFact : IFact 
    where TCmd : ICmd
{
    private readonly Fact2Cmd<TFact, TCmd> _fact2Cmd;

    public Listener(
        ICmdHandler cmdHandler,
        TDriver driver,
        Fact2Cmd<TFact,TCmd> fact2Cmd)
    {
        _fact2Cmd = fact2Cmd;
    }



    protected override Task StartReactingAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    protected override Task StopReactingAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}