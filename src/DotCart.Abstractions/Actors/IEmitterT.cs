using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface IEmitterT<TSpoke, TFactPayload, TMeta>
    where TSpoke : ISpokeT<TSpoke>
    where TFactPayload : IPayload
    where TMeta : IMetaB
{
}