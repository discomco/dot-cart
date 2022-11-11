using DotCart.Context.Effects;

namespace DotCart.Context.Spokes;

public class Spoke : ISpoke
{
    public void Inject(params IReactor[] effects)
    {
        foreach (var effect in effects) effect.SetSpoke(this);
    }
}