using DotCart.Context.Behaviors;

namespace DotCart.Drivers.EventStoreDB;

public record StoreEvent(IEvt Event, long EventNumber);