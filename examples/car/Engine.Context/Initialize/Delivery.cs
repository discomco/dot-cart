using DotCart.Abstractions.Actors;
using DotCart.Context.Spokes;

namespace Engine.Context.Initialize;

public class SpokeBuilder : SpokeBuilderT<Spoke>
{
    public SpokeBuilder(
        Spoke spoke,
        IEnumerable<IActor<Spoke>> actors) : base(
        spoke,
        actors)
    {
    }
}

public interface ISpoke : ISpokeT<Spoke>
{
}

public class Spoke : SpokeT<Spoke>, ISpoke
{
    public Spoke(
        IExchange exchange,
        IProjector projector) : base(exchange, projector)
    {
    }
}