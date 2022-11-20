using DotCart.TestKit.Schema;
using NATS.Client;

namespace DotCart.Drivers.NATS.Tests;

public class TheResponderDriver : NATSResponderDriverT<TheHope>
{
    public TheResponderDriver(IEncodedConnection bus) : base(bus)
    {
    }
}