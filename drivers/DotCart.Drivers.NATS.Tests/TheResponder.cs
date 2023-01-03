using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.TestKit;
using NATS.Client;

namespace DotCart.Drivers.NATS.Tests;

public class TheResponderDriver : NATSResponderDriverT<TheContract.Payload>
{
    public TheResponderDriver(IEncodedConnection bus) : base(bus)
    {
    }
}

public class TheResponder
    : ResponderT<
        IResponderDriverT<TheContract.Payload>,
        TheContract.Payload,
        TheContract.Meta>, ITheResponder
{
    public TheResponder(IResponderDriverT<TheContract.Payload> driver,
        IExchange exchange,
        ICmdHandler cmdHandler,
        Hope2Cmd<TheContract.Payload, TheContract.Meta> hope2Cmd) : base(driver,
        exchange,
        cmdHandler,
        hope2Cmd)
    {
    }
}

public interface ITheResponder : IActor
{
}