using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.TestKit.Behavior;
using DotCart.TestKit.Schema;

namespace DotCart.Drivers.NATS.Tests;

public class TheResponder : ResponderT<IResponderDriverT<TheHope>, TheHope, TheCmd>, ITheResponder
{
    public TheResponder(IExchange exchange,
        IResponderDriverT<TheHope> responderDriver,
        ICmdHandler cmdHandler,
        Hope2Cmd<TheCmd, TheHope> hope2Cmd) : base(exchange,
        responderDriver,
        cmdHandler,
        hope2Cmd)
    {
    }
}

public interface ITheResponder : IActor
{
}