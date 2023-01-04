using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Context.Actors;

public class
    ResponderT<TSpoke, TPayload, TMeta>
    : ResponderT<TPayload, TMeta>, IActorT<TSpoke>
    where TPayload : IPayload
    where TMeta : IEventMeta
{
    public ResponderT(
        IResponderDriverT<TPayload> driver,
        IExchange exchange,
        ICmdHandler cmdHandler,
        Hope2Cmd<TPayload, TMeta> hope2Cmd)
        : base(driver, exchange, cmdHandler, hope2Cmd)
    {
    }
}

public class ResponderT<TPayload, TMeta> : ActorB, IResponderT<TPayload>
    where TMeta : IEventMeta
    where TPayload : IPayload
{
    private readonly ICmdHandler _cmdHandler;

    private readonly Hope2Cmd<TPayload, TMeta> _hope2Cmd;
//    private readonly TDriver _responderDriver;

    public ResponderT(
        IResponderDriverT<TPayload> driver,
        IExchange exchange,
        ICmdHandler cmdHandler,
        Hope2Cmd<TPayload, TMeta> hope2Cmd) : base(exchange)
    {
        Driver = driver;
        _cmdHandler = cmdHandler;
        _hope2Cmd = hope2Cmd;
    }


    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public override async Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        var hope = (HopeT<TPayload>)msg;
        var cmd = _hope2Cmd(hope);
        return await _cmdHandler.HandleAsync(cmd, cancellationToken);
    }

    protected override string GetName()
    {
        return $"{Driver.GetType().Name}<{HopeTopicAtt.Get<TPayload>()}>";
    }


    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken = default)
    {
        ((IResponderDriverT<TPayload>)Driver).SetActor(this);
        return ((IResponderDriverT<TPayload>)Driver).StartRespondingAsync(cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken = default)
    {
        return ((IResponderDriverT<TPayload>)Driver).StopRespondingAsync(cancellationToken);
    }
}