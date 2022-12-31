using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public abstract class ListenerT<TIFact, TICmd, TDriverMsg> 
    : ActorB, ISubscriber
    where TIFact : IFactB
    where TICmd : ICmdB
    where TDriverMsg : class
{
    private readonly ICmdHandler _cmdHandler;
    private readonly IListenerDriverT<TIFact,TDriverMsg> _driver;
    private readonly Fact2Cmd<TICmd, TIFact> _fact2Cmd;

    protected ListenerT(
        IListenerDriverT<TIFact,TDriverMsg> driver,
        IExchange exchange,
        ICmdHandler cmdHandler,
        Fact2Cmd<TICmd, TIFact> fact2Cmd) : base(exchange)
    {
        _driver = driver;
        _cmdHandler = cmdHandler;
        _fact2Cmd = fact2Cmd;
    }

    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken = default)
    {
        var fact = (TIFact)msg;
        var cmd = _fact2Cmd(fact);
        return _cmdHandler.HandleAsync(cmd, cancellationToken);
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>)Task.CompletedTask;
    }
}