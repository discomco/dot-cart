using DotCart.Behavior;
using DotCart.Contract;
using Microsoft.Extensions.Hosting;

namespace DotCart.Effects;


public delegate TCmd Hope2Cmd<in THope, out TCmd>(THope hope) 
    where THope: IHope 
    where TCmd: ICmd;


public abstract class Responder<THope, TCmd> : BackgroundService, IResponder 
    where THope : IHope 
    where TCmd : ICmd 
{
    private readonly ICmdHandler _cmdHandler;
    private readonly Hope2Cmd<THope, TCmd> _hope2Cmd;
    private readonly IResponderDriver _responderDriver;

    public Responder(
        ICmdHandler cmdHandler,
        Hope2Cmd<THope, TCmd> hope2Cmd,
        IResponderDriver responderDriver)
    {
        _cmdHandler = cmdHandler;
        _hope2Cmd = hope2Cmd;
        _responderDriver = responderDriver;
    }

}

public interface IResponderDriver
{
}

public interface IResponder
{
}