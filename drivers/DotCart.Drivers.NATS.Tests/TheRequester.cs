using DotCart.TestKit.Schema;
using NATS.Client;

namespace DotCart.Drivers.NATS.Tests;

public class TheRequester : NATSRequesterT<TheHope>
{
    public TheRequester(IEncodedConnection bus) : base(bus)
    {
    }
}