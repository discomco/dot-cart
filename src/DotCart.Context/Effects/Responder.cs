using DotCart.Context.Behaviors;
using DotCart.Context.Effects.Drivers;
using DotCart.Contract.Dtos;
using DotCart.Core;
using Serilog;

namespace DotCart.Context.Effects;

public delegate TCmd Hope2Cmd<out TCmd, in THope>(THope hope)
    where THope : IHope
    where TCmd : ICmd;

public interface IResponder<TDriver, THope, TCmd> : IReactor
    where TDriver : IResponderDriver<THope>
    where THope : IHope
    where TCmd : ICmd
{
}

public abstract class Responder<TDriver, THope, TCmd> : Reactor, IResponder<TDriver, THope, TCmd>
    where TDriver : IResponderDriver<THope>
    where THope : IHope
    where TCmd : ICmd
{
    private readonly ICmdHandler _cmdHandler;
    private readonly Hope2Cmd<TCmd, THope> _hope2Cmd;
    private readonly IResponderDriver<THope> _responderDriver;

    protected Responder(
        IResponderDriver<THope> responderDriver,
        ICmdHandler cmdHandler,
        Hope2Cmd<TCmd, THope> hope2Cmd)
    {
        _cmdHandler = cmdHandler;
        _hope2Cmd = hope2Cmd;
        _responderDriver = responderDriver;
    }

    public override Task HandleAsync(IMsg msg, CancellationToken cancellationToken)
    {
        var hope = (THope)msg;
        var cmd = _hope2Cmd(hope);
        return _cmdHandler.HandleAsync(cmd, cancellationToken);
    }


    protected override Task StartReactingAsync(CancellationToken cancellationToken)
    {
        Log.Logger.Debug($"[Responder({Topic.Get<THope>()})] has started.");
        _responderDriver.SetReactor(this);
        return _responderDriver.StartRespondingAsync(cancellationToken);
    }

    protected override Task StopReactingAsync(CancellationToken cancellationToken)
    {
        return _responderDriver.StopRespondingAsync(cancellationToken);
    }
}