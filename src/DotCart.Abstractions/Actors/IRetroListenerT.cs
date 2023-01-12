using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface IRetroListenerT<TSpoke, TFactPayload, TMeta, TPipeInfo>
    : IListenerT<TSpoke, Dummy, TMeta, TFactPayload, TPipeInfo>
    where TSpoke : ISpokeT<TSpoke>
    where TFactPayload : IPayload
    where TMeta : IMetaB
{
}