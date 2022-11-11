using DotCart.Contract.Dtos;
using DotCart.Contract.Schemas;

namespace DotCart.Context.Behaviors;

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

public abstract record Cmd<TPayload>(
    string Topic,
    IID AggregateID,
    TPayload Payload
) : ICmd<TPayload>
    where TPayload : IPayload
{
    public TPayload Payload { get; } = Payload;
    public IID AggregateID { get; } = AggregateID;
}