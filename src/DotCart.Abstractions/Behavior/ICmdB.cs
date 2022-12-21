using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public interface ICmdB
{
    IID AggregateID { get; }
}

public interface ICmd : ICmdB
{
    byte[] Data { get; }
    byte[] MetaData { get; }
}

public interface ICmdT<out TPayload> : ICmdB
    where TPayload : IPayload

{
    TPayload Payload { get; }
}