using DotCart.Contract.Dtos;

namespace DotCart.Context.Abstractions.Drivers;

public interface IEmitterDriver : IDriver
{
    Task EmitFactAsync(IFact fact);
}