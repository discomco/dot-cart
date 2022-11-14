using DotCart.Context.Abstractions;
using DotCart.Context.Spokes;

namespace Engine.Context.Start;

public class Spoke : SpokeT<Spoke>
{
    public Spoke(IExchange exchange, IProjector projector) : base(exchange, projector)
    {
    }
}