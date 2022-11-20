using DotCart.Abstractions.Behavior;

namespace DotCart.Abstractions.Drivers;

public record StoreEvent(IEvt Event, long EventNumber);