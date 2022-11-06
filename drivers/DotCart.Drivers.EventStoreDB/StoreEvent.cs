using DotCart.Behavior;

namespace DotCart.Drivers.EventStoreDB;

public record StoreEvent(IEvt Event, long EventNumber);
