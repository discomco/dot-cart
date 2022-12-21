using DotCart.Abstractions.Actors;
using DotCart.Context.Spokes;

namespace DotCart.Drivers.Mediator.Tests;

public class TheSpokeBuilder : SpokeBuilderT<TheSpoke>, ISpokeBuilder<TheSpoke>
{
    public TheSpokeBuilder(
        TheSpoke spoke,
        IEnumerable<IActor<TheSpoke>> actors) : base(
        spoke,
        actors)
    {
    }
}