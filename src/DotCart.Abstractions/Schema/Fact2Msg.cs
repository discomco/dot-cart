namespace DotCart.Abstractions.Schema;

public delegate
    TDriverMsg Fact2Msg<out TDriverMsg, TPayload>(FactT<TPayload> fact)
    where TPayload : IPayload
    where TDriverMsg : class;