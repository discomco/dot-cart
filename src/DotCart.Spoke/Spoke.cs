using DotCart.Effects;

namespace DotCart.Spoke;

public class Spoke : ISpoke
{
    public void Inject(params IReactor[] effects)
    {
        foreach (var effect in effects) effect.SetSpoke(this);
    }
}