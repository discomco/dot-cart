namespace DotCart.Abstractions.Schema;

public delegate HopeT<TPayload> HopeCtorT<TPayload>(string aggId, TPayload payload)
    where TPayload : IPayload;