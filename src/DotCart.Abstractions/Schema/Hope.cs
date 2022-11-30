namespace DotCart.Abstractions.Schema;

public interface IHope : IDto
{
}

public interface IHope<TPayload> : IHope
    where TPayload : IPayload
{
}

public abstract record HopeT<TPayload>(string AggId, TPayload Payload) where TPayload : IPayload
{
    public string AggId { get; private set; } = AggId;
    public TPayload Payload { get; private set; } = Payload;
}