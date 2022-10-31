namespace DotCart.Effects;

public class SpokeBuilder : ISpokeBuilder
{
    private readonly IEnumerable<IEffect> _effects;
    private readonly ISpoke _spoke;

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