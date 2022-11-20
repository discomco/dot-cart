using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IEventStore
{
    Task<IEnumerable<StoreEvent>> ReadEventsAsync(IID id);

    Task<AppendResult> AppendEventAsync<TAggregateId>(IEvt @event) where TAggregateId : IID;

    // Task<IEnumerable<StoreEvent<TAggregateId>>> ReadEventsSlicedAsync<TAggregateId>(TAggregateId id,
    //     int sliceSize, long startPos) where TAggregateId : IIdentity;

    IAsyncEnumerable<StoreEvent> ReadAllEventsAsync(
        CancellationToken cancellationToken = default);
}