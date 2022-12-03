using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Abstractions.Actors;

public delegate TCmd Hope2Cmd<out TCmd, in THope>(THope hope)
    where THope : IHope
    where TCmd : ICmd;

public interface IResponder : IActor
{
}

public interface IResponderT<THope, TCmd> : IResponder
    where THope : IHope
    where TCmd : ICmd
{
}

public class ResponderT<TSpoke, TDriver, THope, TCmd> : ResponderT<TDriver, THope, TCmd>, IActor<TSpoke>
    where TDriver : IResponderDriverT<THope>
    where THope : IHope
    where TCmd : ICmd
{
    public ResponderT(TDriver responderDriver,
        IExchange exchange,
        ICmdHandler cmdHandler,
        Hope2Cmd<TCmd, THope> hope2Cmd) : base(responderDriver,
        exchange,
        cmdHandler,
        hope2Cmd)
    {
    }
}

public class ResponderT<TDriver, THope, TCmd> : ActorB, IResponderT<THope, TCmd>
    where THope : IHope
    where TCmd : ICmd
    where TDriver : IResponderDriverT<THope>
{
    private readonly ICmdHandler _cmdHandler;
    private readonly Hope2Cmd<TCmd, THope> _hope2Cmd;
    private readonly TDriver _responderDriver;

    public ResponderT(
        TDriver responderDriver,
        IExchange exchange,
        ICmdHandler cmdHandler,
        Hope2Cmd<TCmd, THope> hope2Cmd) : base(exchange)
    {
        _responderDriver = responderDriver;
        _cmdHandler = cmdHandler;
        _hope2Cmd = hope2Cmd;
    }


    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public override async Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        var hope = (THope)msg;
        var cmd = _hope2Cmd(hope);
        return await _cmdHandler.HandleAsync(cmd, cancellationToken);
    }

    protected override string GetName()
    {
        return $"{typeof(TDriver).Name}::Responder<{TopicAtt.Get<THope>()}>";
    }


    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken = default)
    {
        _responderDriver.SetActor(this);
        return _responderDriver.StartRespondingAsync(cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken = default)
    {
        return _responderDriver.StopRespondingAsync(cancellationToken);
    }
}