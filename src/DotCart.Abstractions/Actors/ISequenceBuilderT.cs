using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface ISequenceBuilderT<TPayload> where TPayload : IPayload
{
    ISequenceT<TPayload> Build();
}

