using DotCart.Context.Abstractions;
using DotCart.Context.Spokes;

namespace Engine.Context.Start;

public class SpokeBuilder : SpokeBuilderT<Spoke>
{
    public SpokeBuilder(
        Spoke spoke,
        IEnumerable<IActor<Spoke>> actors) : base(spoke, actors)
    {
    }
}

public class Spoke : SpokeT<Spoke>
{
    public Spoke(
        IExchange exchange,
        IProjector projector) : base(exchange, projector)
    {
    }
}