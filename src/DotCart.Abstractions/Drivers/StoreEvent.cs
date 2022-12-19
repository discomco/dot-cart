using DotCart.Abstractions.Behavior;

namespace DotCart.Abstractions.Drivers;

public record StoreEvent(IEvtB Event, long EventNumber);