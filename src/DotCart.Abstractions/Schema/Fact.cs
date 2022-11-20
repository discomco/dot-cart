namespace DotCart.Abstractions.Schema;

public interface IFact : IDto
{
}

public interface IFact<TPayload> : IFact
    where TPayload : IPayload
{
}