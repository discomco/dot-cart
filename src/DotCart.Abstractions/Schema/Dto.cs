namespace DotCart.Abstractions.Schema;

public abstract record Dto<TPayload>(string AggId, TPayload Payload) : IDto
{
    public TPayload Payload { get; } = Payload;
    public string AggId { get; set; } = AggId;
}

public interface IDto : IMsg
{
    string AggId { get; set; }
}