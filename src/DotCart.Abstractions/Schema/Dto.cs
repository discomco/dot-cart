namespace DotCart.Abstractions.Schema;

public abstract record Dto<TPayload>(string AggId, TPayload Payload) : IDto;

public interface IDto : IMsg
{
    string AggId { get; }
}