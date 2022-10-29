namespace DotCart.Contract;

public interface IHope : IDto
{
}

public abstract record Hope(string AggId, byte[] Data) : Dto(AggId, Data), IHope;