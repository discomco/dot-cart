using DotCart.Abstractions.Actors;
using DotCart.Core;

namespace DotCart.Drivers.Mediator.Tests;

[Name("the_spoke")]
public class TheSpoke : SpokeT<TheSpoke>
{
    public TheSpoke(IExchange exchange, IProjector projector) : base(exchange, projector)
    {
    }
}