using NATS.Client;

namespace DotCart.Drivers.NATS.Tests;

public class TheRequesterDriver : NATSRequesterDriverT
{
    public TheRequesterDriver(IEncodedConnection bus) : base(bus)
    {
    }
}