using DotCart.Contract;

namespace DotCart.Effects;

public interface IEmitter : IReactor
{
    Task Emit(IFact fact);
}