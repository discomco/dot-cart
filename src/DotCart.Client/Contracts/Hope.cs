namespace DotCart.Client.Contracts;

public interface IHope : IDto
{
}

public interface IHope<TPayload> : IHope
    where TPayload : IPayload
{
}