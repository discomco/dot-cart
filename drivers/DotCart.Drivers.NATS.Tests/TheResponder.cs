using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Context.Actors;
using DotCart.TestKit;
using NATS.Client;

namespace DotCart.Drivers.NATS.Tests;

public class TheResponderDriver : NATSResponderDriverT<TheContract.Payload>
{
    public TheResponderDriver(INatsClientConnectionFactory connectionFactory, Action<Options> configureOptions)
        : base(connectionFactory, configureOptions)
    {
    }
}

public class TheResponder
    : ResponderT<
        TheSpoke,
        TheContract.Payload,
        TheContract.Meta>, ITheResponder
{
    public TheResponder(
        INATSResponderDriverT<TheContract.Payload> driver,
        IExchange exchange,
        ISequenceBuilderT<TheContract.Payload> builder,
        Hope2Cmd<TheContract.Payload, TheContract.Meta> hope2Cmd)
        : base(driver, exchange, builder, hope2Cmd)
    {
    }
}

public interface ITheResponder : IActor
{
}