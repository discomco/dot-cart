using DotCart.TestKit.Schema;
using NATS.Client;

namespace DotCart.Drivers.NATS.Tests;

public class TheRequesterDriver : NATSRequesterDriverT<TheHope>
{
    public TheRequesterDriver(IEncodedConnection bus) : base(bus)
    {
    }
}