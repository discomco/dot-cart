using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Contract.Dtos;
using DotCart.Core;
using Serilog;

namespace DotCart.Context.Effects;

public delegate TCmd Hope2Cmd<out TCmd, in THope>(THope hope)
    where THope : IHope
    where TCmd : ICmd;

public interface IResponder<TDriver, THope, TCmd> : IActor
    where TDriver : IResponderDriver<THope>
    where THope : IHope
    where TCmd : ICmd
{
}

public abstract class ResponderT<TSpoke, TDriver, THope, TCmd> : ActorT<TSpoke>, IResponder<TDriver, THope, TCmd>
    where TDriver : IResponderDriver<THope>
    where THope : IHope
    where TCmd : ICmd
    where TSpoke : ISpokeT<TSpoke>
{
    private readonly ICmdHandler _cmdHandler;
    private readonly Hope2Cmd<TCmd, THope> _hope2Cmd;
    private readonly IResponderDriver<THope> _responderDriver;

    protected ResponderT(
        IExchange exchange,
        IResponderDriver<THope> responderDriver,
        ICmdHandler cmdHandler,
        Hope2Cmd<TCmd, THope> hope2Cmd) : base(exchange)
    {
        _cmdHandler = cmdHandler;
        _hope2Cmd = hope2Cmd;
        _responderDriver = responderDriver;
    }

    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken)
    {
        var hope = (THope)msg;
        var cmd = _hope2Cmd(hope);
        return _cmdHandler.HandleAsync(cmd, cancellationToken);
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>)Task.CompletedTask;
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