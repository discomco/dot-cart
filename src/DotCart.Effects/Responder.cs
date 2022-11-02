using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Effects.Drivers;


namespace DotCart.Effects;


public delegate TCmd Hope2Cmd<in THope, out TCmd>(THope hope) 
    where THope: IHope 
    where TCmd: ICmd;

public interface IResponder<TDriver, THope, TCmd>: IReactor
    where TDriver: IResponderDriver<THope>
    where THope : IHope 
    where TCmd : ICmd
{
}

public abstract class Responder<TDriver,THope, TCmd> : Reactor, IResponder<TDriver, THope, TCmd>
    where TDriver: IResponderDriver<THope>
    where THope : IHope 
    where TCmd : ICmd 
{
    private readonly ICmdHandler _cmdHandler;
    private readonly Hope2Cmd<THope, TCmd> _hope2Cmd;
    private readonly IResponderDriver<THope> _responderDriver;

    public Responder(
        IResponderDriver<THope> responderDriver,
        ICmdHandler cmdHandler,
        Hope2Cmd<THope, TCmd> hope2Cmd)
    {
        _cmdHandler = cmdHandler;
        _hope2Cmd = hope2Cmd;
        _responderDriver = responderDriver;
    }


    protected override Task StartReactingAsync(CancellationToken cancellationToken)
    {
        _responderDriver.SetReactor(this);
        return _responderDriver.StartRespondingAsync(cancellationToken);
    }

    protected override Task StopReactingAsync(CancellationToken cancellationToken)
    {
        return _responderDriver.StopRespondingAsync(cancellationToken);
    }

    public override Task HandleAsync(IMsg msg)
    {
        return Task.Run(() =>
        {
            var hope = (THope)msg;
            var cmd = _hope2Cmd(hope);
            _cmdHandler.Handle(cmd);
        });
    }
}