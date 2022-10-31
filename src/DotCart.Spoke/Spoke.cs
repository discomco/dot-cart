

using DotCart.Effects;

namespace DotCart.Spoke;

public class Spoke : ISpoke
{
    public void Inject(params IEffect[] effects)
    {
        foreach (var effect in effects)
        {
            effect.SetSpoke(this);
        }
    }
}
