using DotCart.Abstractions.Behavior;

namespace DotCart.TestKit.Contract;

public record TheMeta(string AggregateType, string AggregateId) 
    : EventMeta(AggregateType, AggregateId);
