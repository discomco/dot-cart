namespace DotCart.Abstractions.Schema;

public interface IFact : IDto
{
}

public interface IFact<TPayload> : IFact
    where TPayload : IPayload
{
}

public abstract record FactT<TPayload>(string AggId, TPayload Payload) : IFact<TPayload>
    where TPayload : IPayload;