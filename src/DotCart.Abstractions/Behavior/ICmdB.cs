using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public interface ICmdB
{
    IID AggregateID { get; }
    string CmdType { get; }
}

public interface ICmd : ICmdB
{
    byte[] Data { get; }
    byte[] MetaData { get; }
}

public interface ICmdT<out TPayload, out TMeta> : ICmdB
    where TPayload : IPayload
    where TMeta : IEventMeta

{
    TPayload Payload { get; }
    TMeta Meta { get; }
}