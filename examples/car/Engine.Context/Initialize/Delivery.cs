using DotCart.Context.Abstractions;
using DotCart.Context.Spokes;

namespace Engine.Context.Initialize;

public class SpokeBuilder : SpokeBuilderT<Spoke>
{
    public SpokeBuilder(IExchange exchange,
        IProjector projector,
        Spoke spoke,
        IEnumerable<IActor<Spoke>> actors) : base(exchange,
        projector,
        spoke,
        actors)
    {
    }
}


public interface ISpoke: ISpokeT<Spoke> {}

public class Spoke : SpokeT<Spoke>, ISpoke
{
    public Spoke(IExchange exchange) : base(exchange)
    {
    }
}