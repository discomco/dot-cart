using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public interface ICmd
{
    IID AggregateID { get; }
    string Topic { get; }
}

public interface ICmd<out TPayload> : ICmd
    where TPayload : IPayload

{
    TPayload Payload { get; }
}