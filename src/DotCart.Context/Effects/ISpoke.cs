namespace DotCart.Context.Effects;

public interface ISpoke
{
    void Inject(params IReactor[] effects);
}