using DotCart.Context.Abstractions;

namespace DotCart.Drivers.EventStoreDB;

public record StoreEvent(IEvt Event, long EventNumber);