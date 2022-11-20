using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IEmitterDriver : IDriver
{
    Task EmitFactAsync(IFact fact);
}