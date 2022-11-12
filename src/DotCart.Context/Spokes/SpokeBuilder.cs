using DotCart.Context.Effects;

namespace DotCart.Context.Spokes;

public abstract class SpokeBuilder<TSpoke> : ISpokeBuilder<TSpoke>
    where TSpoke : ISpoke<TSpoke>
{
    private readonly IEnumerable<IReactor<TSpoke>> _reactors;
    private readonly TSpoke _spoke;

    protected SpokeBuilder(
        TSpoke spoke,
        IEnumerable<IReactor<TSpoke>> reactors)

    {
        _spoke = spoke;
        _reactors = reactors;
    }

    public TSpoke Build()
    {
        _spoke.Inject(_reactors.ToArray());
        return _spoke;
    }
}

public interface ISpokeBuilder<out TSpoke> where TSpoke : ISpoke<TSpoke>
{
    TSpoke Build();
}