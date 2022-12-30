namespace DotCart.Abstractions.Schema;

public delegate FactT<TPayload> FactCtorT<TPayload>(string aggId, TPayload payload)
    where TPayload : IPayload;