using DotCart.Context.Abstractions;
using DotCart.Context.Spokes;
using DotCart.Drivers.InMem;

namespace DotCart.Drivers.Mediator.Tests;

public class SpokeBuilder : SpokeBuilderT<ISpokeT<Spoke>>, ISpokeBuilder<ISpokeT<Spoke>>
{
    public SpokeBuilder(IExchange exchange,
        IMemProjector projector,
        ISpokeT<Spoke> spoke,
        IEnumerable<IActor<ISpokeT<Spoke>>> actors) : base(exchange,
        projector,
        spoke,
        actors)
    {
    }
}