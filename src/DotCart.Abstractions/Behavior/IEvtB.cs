using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public interface IEvtB : IMsg
{
    string AggregateId { get; }
    string EventId { get; }
    byte[] MetaData { get; }
    long Version { get; }
    string Topic { get; }
    DateTime TimeStamp { get; }
    byte[] Data { get; }
    bool IsCommitted { get; }
    void SetIsCommitted(bool isCommitted);
    void SetVersion(long version);
    void SetTimeStamp(DateTime timeStamp);
    void SetData(byte[] data);
    void SetEventType(string eventType);
    void SetMetaData(byte[] metaData);

    void SetMeta<TMeta>(TMeta meta) where TMeta : IEventMeta;
    TMeta GetMeta<TMeta>() where TMeta : IEventMeta;
    TPayload GetPayload<TPayload>() where TPayload : IPayload;
    void SetPayload<TPayload>(TPayload payload) where TPayload : IPayload;
}