namespace DotCart.Abstractions.Schema;

public interface IFact : IDto
{
}

public interface IFact<TPayload> : IFact
    where TPayload : IPayload
{
}

public abstract record FactT<TPayload>(string AggId, TPayload Payload)
    where TPayload : IPayload;