using DotCart.TestKit;
using NATS.Client;

namespace DotCart.Drivers.NATS.Tests;

public class TheRequester : NATSRequesterT<TheContract.Payload>
{
    public TheRequester(INatsClientConnectionFactory connectionFactory, Action<Options> configureOptions) 
        : base(connectionFactory, configureOptions)
    {
    }
}