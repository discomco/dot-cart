using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

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

public abstract class ResponderT<THope, TCmd> : ActorB, IResponderT<THope, TCmd>
    where THope : IHope
    where TCmd : ICmd
{
    private readonly ICmdHandler _cmdHandler;
    private readonly Hope2Cmd<TCmd, THope> _hope2Cmd;

    protected ResponderT(
        IExchange exchange,
        ICmdHandler cmdHandler,
        Hope2Cmd<TCmd, THope> hope2Cmd) : base(exchange)
    {
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


    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}