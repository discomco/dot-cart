using DotCart.Abstractions.Actors;

namespace DotCart.Context.Spokes;

public class SpokeBuilderT<TSpoke>
    : ISpokeBuilder<TSpoke>
    where TSpoke : ISpokeT<TSpoke>
{
    private readonly IEnumerable<IActorT<TSpoke>> _actors;
    private readonly TSpoke _spoke;

    public SpokeBuilderT(
        TSpoke spoke,
        IEnumerable<IActorT<TSpoke>> actors)
    {
        _spoke = spoke;
        _actors = actors;
    }

    public TSpoke Build()
    {
        _spoke.InjectActors(_actors.ToArray());
        return _spoke;
    }
}

public interface ISpokeBuilder<out TSpoke> : ISpokeBuilderB
    where TSpoke : ISpokeT<TSpoke>
{
    TSpoke Build();
}

public interface ISpokeBuilderB
{
}