namespace DotCart.Abstractions.Schema;

public delegate FactT<TPayload, TMeta> FactCtorT<TPayload, TMeta>(string aggId, TPayload payload, TMeta meta)
    where TPayload : IPayload;