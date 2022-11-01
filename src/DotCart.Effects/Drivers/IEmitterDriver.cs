using DotCart.Contract;

namespace DotCart.Effects.Drivers;

public interface IEmitterDriver : IDriver
{
    Task EmitFact(IFact fact);
}