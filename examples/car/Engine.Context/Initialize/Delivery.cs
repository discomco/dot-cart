using DotCart.Context.Effects;
using DotCart.Context.Spokes;

namespace Engine.Context.Initialize;

public class SpokeBuilder : SpokeBuilder<Spoke>
{
    public SpokeBuilder(Spoke spoke, IEnumerable<IReactor<Spoke>> reactors) : base(spoke, reactors)
    {
    }
}

public class Spoke : Spoke<Spoke>
{
}