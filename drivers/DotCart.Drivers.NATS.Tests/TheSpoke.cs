using DotCart.Abstractions.Actors;
using DotCart.Actors;

namespace DotCart.Drivers.NATS.Tests;

public class TheSpoke : SpokeT<TheSpoke>
{
    public TheSpoke(
        IExchange exchange,
        IProjector projector) : base(exchange, projector)
    {
    }
}