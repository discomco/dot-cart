using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public interface IEvt : IMsg
{
    ID AggregateID { get; }
    byte[] MetaData { get; set; }
    long Version { get; set; }
    string Topic { get; }
    void SetVersion(long version);
    void SetMetaPayload<TPayload>(TPayload payload);
    TPayload GetMetaPayload<TPayload>();
}