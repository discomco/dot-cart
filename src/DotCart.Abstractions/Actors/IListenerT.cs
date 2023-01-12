using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface IListenerT<TSpoke, TCmdPayload, TMeta, TFactPayload, TPipeInfo>
    : IListenerB
    where TSpoke : ISpokeT<TSpoke>
    where TCmdPayload : IPayload
    where TFactPayload : IPayload
    where TMeta : IMetaB
{
}