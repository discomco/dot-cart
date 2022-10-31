using DotCart.Contract;

namespace DotCart.Effects;

public interface IEmitter : IEffect
{
    Task Emit(IFact fact);
}