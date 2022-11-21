using DotCart.Core;

namespace DotCart.Abstractions.Schema;

public delegate THope GenerateHope<out THope>()
    where THope : IHope;

public interface IHope : IDto
{
}

public interface IHope<TPayload> : IHope
    where TPayload : IPayload
{
}

public abstract record HopeT<TPayload>(string AggId, TPayload Payload) : Dto(AggId, Payload.ToBytes())
    where TPayload : IPayload
{
    public TPayload Payload
    {
        get => GetPayload<TPayload>();
        set => SetPayload(value);
    }
}