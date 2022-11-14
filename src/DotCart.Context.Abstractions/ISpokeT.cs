namespace DotCart.Context.Abstractions;

public interface ISpoke
{
}

public interface ISpokeT<out TSpoke> : ISpoke
    where TSpoke : ISpokeT<TSpoke>
{
    void Inject(params IActor<TSpoke>[] reactors);
}