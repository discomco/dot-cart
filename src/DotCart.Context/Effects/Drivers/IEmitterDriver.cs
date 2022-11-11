using DotCart.Client.Contracts;

namespace DotCart.Context.Effects.Drivers;

public interface IEmitterDriver : IDriver
{
    Task EmitFact(IFact fact);
}