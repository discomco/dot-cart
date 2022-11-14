using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Contract.Dtos;

namespace DotCart.Context.Effects;

public delegate TCmd Fact2Cmd<in TFact, out TCmd>(TFact fact)
    where TFact : IFact
    where TCmd : ICmd;

public interface IListener : IActor
{
}

public abstract class Listener<TSpoke, TDriver, TFact, TCmd> : ActorT<TSpoke>, IListener
    where TDriver : IListenerDriver
    where TFact : IFact
    where TCmd : ICmd
    where TSpoke : ISpokeT<TSpoke>
{
    private readonly ICmdHandler _cmdHandler;
    private readonly TDriver _driver;
    private readonly Fact2Cmd<TFact, TCmd> _fact2Cmd;

    protected Listener(
        IExchange exchange,
        ICmdHandler cmdHandler,
        TDriver driver,
        Fact2Cmd<TFact, TCmd> fact2Cmd) : base(exchange)
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