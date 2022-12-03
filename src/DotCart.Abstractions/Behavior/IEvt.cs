using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public interface IEvt : IMsg
{
    string AggregateId { get; set; }
    string EventId { get; set; }
    byte[] MetaData { get; set; }
    long Version { get; set; }
    string Topic { get; }
    DateTime TimeStamp { get; }
    byte[] Data { get; }
    void SetVersion(long version);
    void SetMetaPayload<TPayload>(TPayload payload);
    TPayload GetMetaPayload<TPayload>();
    void SetTimeStamp(DateTime timeStamp);
    TPayload GetPayload<TPayload>() where TPayload : IPayload;
    void SetPayload<TPayload>(TPayload payload) where TPayload : IPayload;
    void SetData(byte[] data);
}