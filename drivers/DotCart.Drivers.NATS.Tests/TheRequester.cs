using DotCart.TestKit.Mocks;
using NATS.Client;

namespace DotCart.Drivers.NATS.Tests;

public class TheRequester : NATSRequesterT<TheContract.Payload>
{
    public TheRequester(INatsClientConnectionFactory connectionFactory, Action<Options> options)
        : base(connectionFactory, options)
    {
    }
}