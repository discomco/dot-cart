using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Contract;

public interface IHopeB
    : IDto;

public interface IHopeT<TPayload>
    : IHopeB
    where TPayload : IPayload;

public record HopeT<TPayload>(string AggId, TPayload Payload)
    : Dto<TPayload>(AggId, Payload)
    where TPayload : IPayload
{
    public static HopeT<TPayload> New(string aggId, TPayload payload)
    {
        return new HopeT<TPayload>(aggId, payload);
    }
}