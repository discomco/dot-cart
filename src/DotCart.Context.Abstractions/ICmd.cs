using DotCart.Contract.Dtos;
using DotCart.Contract.Schemas;

namespace DotCart.Context.Abstractions;

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