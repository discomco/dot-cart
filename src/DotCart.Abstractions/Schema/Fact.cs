namespace DotCart.Abstractions.Schema;

public interface IFactB : IDto
{
}

public interface IFactT<TPayload> : IFactB
    where TPayload : IPayload
{
}

public abstract record FactT<TPayload>(string AggId, TPayload Payload)
    : Dto<TPayload>(AggId, Payload), IFactT<TPayload>
    where TPayload : IPayload;
