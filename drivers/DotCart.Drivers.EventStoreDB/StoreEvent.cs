using DotCart.Behavior;

namespace DotCart.Drivers.EventStoreDB;

public record StoreEvent
{
    public long EventNumber { get; init; }
    public IEvt Event { get; init; }
}
