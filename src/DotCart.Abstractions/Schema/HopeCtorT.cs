namespace DotCart.Abstractions.Schema;

public delegate THope HopeCtorT<out THope, in TPayload>(string aggId, TPayload payload)
    where THope : IHope<TPayload>
    where TPayload : IPayload;