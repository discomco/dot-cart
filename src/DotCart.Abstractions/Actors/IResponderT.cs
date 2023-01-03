using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface IResponder : IActor
{
}

public interface IResponderT<TPayload> : IResponder
    where TPayload : IPayload
{
}