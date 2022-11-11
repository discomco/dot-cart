namespace DotCart.Contract.Dtos;

public interface IFact : IDto
{
}

public interface IFact<TPayload> : IFact
    where TPayload : IPayload
{
}