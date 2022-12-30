using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.TestKit;
using NATS.Client;

namespace DotCart.Drivers.NATS.Tests;

public class TheResponderDriver : NATSResponderDriverT<TheContract.Hope>
{
    public TheResponderDriver(IEncodedConnection bus) : base(bus)
    {
    }
}

public class TheResponder : ResponderT<IResponderDriverT<TheContract.Hope>, TheContract.Hope, TheBehavior.Cmd>,
    ITheResponder
{
    public TheResponder(IResponderDriverT<TheContract.Hope> responderDriver,
        IExchange exchange,
        ICmdHandler cmdHandler,
        Hope2Cmd<TheBehavior.Cmd, TheContract.Hope> hope2Cmd) : base(responderDriver,
        exchange,
        cmdHandler,
        hope2Cmd)
    {
    }
}

public interface ITheResponder : IActor
{
}