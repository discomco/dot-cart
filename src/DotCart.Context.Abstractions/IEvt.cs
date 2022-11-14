using DotCart.Contract.Dtos;
using DotCart.Contract.Schemas;

namespace DotCart.Context.Abstractions;

public interface IEvt : IMsg
{
    ID AggregateID { get; }
    byte[] MetaData { get; set; }
    long Version { get; set; }
    void SetVersion(long version);
    void SetMetaPayload<TPayload>(TPayload payload);
    TPayload GetMetaPayload<TPayload>();
}