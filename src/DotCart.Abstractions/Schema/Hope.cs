namespace DotCart.Abstractions.Schema;

public interface IHopeB : IDto
{
}

public interface IHopeT<TPayload> : IHopeB
    where TPayload : IPayload
{
}

public abstract record HopeT<TPayload>(string AggId, TPayload Payload) 
    : Dto<TPayload>(AggId, Payload)
    where TPayload : IPayload;
