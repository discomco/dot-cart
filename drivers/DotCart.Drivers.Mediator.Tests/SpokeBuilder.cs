using DotCart.Abstractions.Actors;
using DotCart.Context.Spokes;

namespace DotCart.Drivers.Mediator.Tests;

public class SpokeBuilder : SpokeBuilderT<ISpokeT<Spoke>>, ISpokeBuilder<ISpokeT<Spoke>>
{
    public SpokeBuilder(
        ISpokeT<Spoke> spoke,
        IEnumerable<IActor<ISpokeT<Spoke>>> actors) : base(
        spoke,
        actors)
    {
    }
}