using DotCart.Abstractions.Actors;
using DotCart.TestKit.Behavior;
using DotCart.TestKit.Schema;
using NATS.Client;

namespace DotCart.Drivers.NATS.Tests;

public class TheResponder : NATSResponderT<TheHope, TheCmd>, ITheResponder
{
    public TheResponder(
        IEncodedConnection bus,
        IExchange exchange,
        ICmdHandler cmdHandler,
        Hope2Cmd<TheCmd, TheHope> hope2Cmd) : base(bus,
        exchange,
        cmdHandler,
        hope2Cmd)
    {
    }
}

public interface ITheResponder : IActor
{
}