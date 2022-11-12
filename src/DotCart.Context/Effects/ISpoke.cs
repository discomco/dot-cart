namespace DotCart.Context.Effects;

public interface ISpoke<out TSpoke>
    where TSpoke : ISpoke<TSpoke>
{
    void Inject(params IReactor<TSpoke>[] reactors);
}