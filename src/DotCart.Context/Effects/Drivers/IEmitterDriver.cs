using DotCart.Contract.Dtos;

namespace DotCart.Context.Effects.Drivers;

public interface IEmitterDriver : IDriver
{
    Task EmitFact(IFact fact);
}