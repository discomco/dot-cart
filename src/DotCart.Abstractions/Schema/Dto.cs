namespace DotCart.Abstractions.Schema;

public abstract record Dto<TPayload>(string AggId, TPayload Payload)
    : IDtoT<TPayload>
{
    public TPayload Payload { get; } = Payload;
    public string AggId { get; set; } = AggId;
}

public interface IDto : IMsg
{
    string AggId { get; set; }
}

public interface IDtoT<out TPayload> : IDto
{
    TPayload Payload { get; }
}