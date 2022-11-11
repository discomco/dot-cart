namespace DotCart.Client.Contracts;

public interface IFact : IDto
{
}

public interface IFact<TPayload> : IFact
    where TPayload : IPayload
{
}