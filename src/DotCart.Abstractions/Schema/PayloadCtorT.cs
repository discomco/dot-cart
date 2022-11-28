namespace DotCart.Abstractions.Schema;

public delegate TPayload PayloadCtorT<out TPayload>() 
    where TPayload : IPayload;