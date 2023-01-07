using DotCart.Abstractions.Actors;
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
        TheContext.IPipeInfo>, ITheResponder
{
    public TheResponder(
        INATSResponderDriverT<TheContract.Payload> driver,
        IExchange exchange,
        IPipeBuilderT<TheContext.IPipeInfo, TheContract.Payload> builder)
        : base(driver, exchange, builder)
    {
    }
}

public interface ITheResponder : IActor
{
}