using DotCart.Behavior;
using DotCart.Spoke;

namespace DotCart.Effects;

public class SpokeBuilder : ISpokeBuilder
{
    private readonly ISpoke _spoke;
    private readonly IEnumerable<IEffect> _effects;

    public SpokeBuilder(
        ISpoke spoke,
        IEnumerable<IEffect> effects)
    {
        _spoke = spoke;
        _spoke.Inject(effects.ToArray());
    }

    public ISpoke Build()
    {
        return _spoke;
    }
}



public interface ISpokeBuilder
{
    ISpoke Build();
}