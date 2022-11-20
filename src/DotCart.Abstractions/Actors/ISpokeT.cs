namespace DotCart.Abstractions.Actors;

public interface ISpokeB
{
}

public interface ISpokeT<out TSpoke> : ISpokeB
    where TSpoke : ISpokeT<TSpoke>
{
    void InjectActors(params IActor<TSpoke>[] actors);
}