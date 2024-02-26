using DotCart.Abstractions.Actors;
using DotCart.Spokes;

namespace DotCart.Context.Tests.Actors;

public class TheSpokeBuilder : SpokeBuilderT<TheSpoke>
{
    public TheSpokeBuilder(TheSpoke spoke,
        IEnumerable<IActorT<TheSpoke>> actors) : base(
        spoke,
        actors)
    {
    }
}