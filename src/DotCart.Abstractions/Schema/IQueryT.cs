namespace DotCart.Abstractions.Schema;

public interface IQueryB : IDto
{
}

public interface IQueryT<TPayload>
    : IQueryB
    where TPayload : IPayload
{
}