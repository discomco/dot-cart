using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface IProjectionB : IActorB
{
}

public interface IProjectionT<TDbInfo, TDoc, TPayload, TMeta, TID>
    : IProjectionB
    where TDoc : IState
    where TPayload : IPayload
    where TMeta : IMetaB
    where TDbInfo : IDbInfoB
    where TID : IID
{
}