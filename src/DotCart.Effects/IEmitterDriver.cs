using DotCart.Contract;

namespace DotCart.Effects;

public interface IEmitterDriver : IDriver
{
    Task EmitFact(IFact fact);
}