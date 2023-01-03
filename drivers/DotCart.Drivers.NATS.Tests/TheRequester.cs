using DotCart.TestKit;
using NATS.Client;

namespace DotCart.Drivers.NATS.Tests;

public class TheRequester : NATSRequesterT<TheContract.Payload>
{
    public TheRequester(IEncodedConnection bus) : base(bus)
    {
    }
}