namespace DotCart.Abstractions.Schema;

public abstract record Dto<TPayload>(string AggId, TPayload Payload) : IDto
{
    public string AggId { get; set; } = AggId;
    public TPayload Payload { get; } = Payload;
}

public interface IDto : IMsg
{
    string AggId { get; set; }
}