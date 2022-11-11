namespace DotCart.Contract.Dtos;

public interface IHope : IDto
{
}

public interface IHope<TPayload> : IHope
    where TPayload : IPayload
{
}