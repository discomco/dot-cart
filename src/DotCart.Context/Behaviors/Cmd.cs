using DotCart.Context.Abstractions;
using DotCart.Contract.Dtos;
using DotCart.Contract.Schemas;

namespace DotCart.Context.Behaviors;

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