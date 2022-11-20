using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Serilog;

namespace DotCart.Abstractions.Actors;

public delegate TCmd Hope2Cmd<out TCmd, in THope>(THope hope)
    where THope : IHope
    where TCmd : ICmd;

public interface IResponder : IActor
{
}

public interface IResponderT3<TDriver, THope, TCmd> : IResponder
    where TDriver : IResponderDriverT<THope>
    where THope : IHope
    where TCmd : ICmd
{
}

public class ResponderT<TResponderDriver, THope, TCmd> : ActorB,
    IResponderT3<TResponderDriver, THope, TCmd>
    where TResponderDriver : IResponderDriverT<THope>
    where THope : IHope
    where TCmd : ICmd
{
    private readonly ICmdHandler _cmdHandler;
    private readonly Hope2Cmd<TCmd, THope> _hope2Cmd;
    private readonly TResponderDriver _responderDriver;

    protected ResponderT(
        IExchange exchange,
        TResponderDriver responderDriver,
        ICmdHandler cmdHandler,
        Hope2Cmd<TCmd, THope> hope2Cmd) : base(exchange)
    {
        _cmdHandler = cmdHandler;
        _hope2Cmd = hope2Cmd;
        _responderDriver = responderDriver;
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


    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken)
    {
        Log.Logger.Debug($"[Responder({TopicAtt.Get<THope>()})] has started.");
        _responderDriver.SetActor(this);
        return _responderDriver.StartRespondingAsync(cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken)
    {
        return _responderDriver.StopRespondingAsync(cancellationToken);
    }
}