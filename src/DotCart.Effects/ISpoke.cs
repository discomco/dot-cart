namespace DotCart.Effects;

public interface ISpoke
{
    void Inject(params IReactor[] effects);
}