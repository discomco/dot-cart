namespace DotCart.Abstractions.Actors;

public interface ISpokeB
{
    ComponentStatus Status { get; set; }
}

public interface ISpokeT<out TSpoke> : ISpokeB
    where TSpoke : ISpokeT<TSpoke>
{
    void InjectActors(params IActor<TSpoke>[] actors);
}