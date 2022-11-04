using DotCart.Effects;

namespace DotCart.Spoke;

public class SpokeBuilder : ISpokeBuilder
{
    private readonly IEnumerable<IReactor> _effects;
    private readonly ISpoke _spoke;

    public SpokeBuilder(
        ISpoke spoke,
        IEnumerable<IReactor> effects)
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