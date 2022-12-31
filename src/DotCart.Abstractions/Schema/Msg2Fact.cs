namespace DotCart.Abstractions.Schema;

public delegate FactT<TPayload> Msg2Fact<TPayload, in TDriverMsg>(TDriverMsg msg)
    where TPayload : IPayload
    where TDriverMsg : class;