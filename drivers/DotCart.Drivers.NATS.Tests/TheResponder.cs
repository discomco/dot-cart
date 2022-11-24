using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.TestKit.Behavior;
using DotCart.TestKit.Schema;
using NATS.Client;

namespace DotCart.Drivers.NATS.Tests;

public class TheResponderDriver : NATSResponderDriverT<TheHope>
{
    public TheResponderDriver(IEncodedConnection bus) : base(bus)
    {
    }
}

public class TheResponder : ResponderT<IResponderDriverT<TheHope>, TheHope, TheCmd>, ITheResponder
{
    public TheResponder(IResponderDriverT<TheHope> responderDriver,
        IExchange exchange,
        ICmdHandler cmdHandler,
        Hope2Cmd<TheCmd, TheHope> hope2Cmd) : base(responderDriver,
        exchange,
        cmdHandler,
        hope2Cmd)
    {
    }
}

public interface ITheResponder : IActor
{
}