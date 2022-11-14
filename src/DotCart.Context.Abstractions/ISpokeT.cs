namespace DotCart.Context.Abstractions;

public interface ISpokeB
{
}

public interface ISpokeT<out TSpoke> : ISpokeB
    where TSpoke : ISpokeT<TSpoke>
{
    void InjectActors(params IActor<TSpoke>[] actors);
}