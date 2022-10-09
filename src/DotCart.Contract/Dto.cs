using System.Text.Json;
using DotCart.Schema;

namespace DotCart.Contract;

public record Dto<TPayload>(string AggId, byte[] Data) : IDto<TPayload> where TPayload:IPayload
{
    public string AggId { get; }
    public byte[] Data { get; private set; }


    public TPayload Payload => JsonSerializer.Deserialize<TPayload>(Data);
    
    
    public void SetPayload(TPayload payload)
    {
        Data = JsonSerializer.SerializeToUtf8Bytes(payload);
    }
}

public interface IDto {}

public interface IDto<TPayload> : IDto where TPayload: IPayload
{
    string AggId { get; }
    byte[] Data { get;  }
    TPayload Payload { get; }
    void SetPayload(TPayload payload);
}