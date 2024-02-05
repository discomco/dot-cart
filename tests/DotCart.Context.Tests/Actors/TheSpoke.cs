using DotCart.Abstractions.Actors;
using DotCart.Core;

namespace DotCart.Context.Tests.Actors;

[Name("the_spoke")]
public class TheSpoke : SpokeT<TheSpoke>
{
    public TheSpoke(IExchange exchange, IProjector projector) : base(exchange, projector)
    {
    }
}