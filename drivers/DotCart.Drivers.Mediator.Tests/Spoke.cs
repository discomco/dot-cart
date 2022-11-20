using DotCart.Abstractions.Actors;

namespace DotCart.Drivers.Mediator.Tests;

public class Spoke : SpokeT<Spoke>
{
    public Spoke(IExchange exchange, IProjector projector) : base(exchange, projector)
    {
    }
}