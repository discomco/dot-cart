namespace DotCart.Abstractions.Schema;

public delegate TFact FactCtorT<out TFact, in TPayload>(string aggId, TPayload payload)
    where TFact : IFactT<TPayload>
    where TPayload : IPayload;