using DotCart.Context.Behaviors;
using DotCart.Context.Effects.Drivers;
using DotCart.Contract.Schemas;

namespace DotCart.Drivers.EventStoreDB;

public interface IEventStore
{
    Task<IEnumerable<StoreEvent>> ReadEventsAsync(IID id);

    Task<AppendResult> AppendEventAsync<TAggregateId>(IEvt @event) where TAggregateId : IID;

    // Task<IEnumerable<StoreEvent<TAggregateId>>> ReadEventsSlicedAsync<TAggregateId>(TAggregateId id,
    //     int sliceSize, long startPos) where TAggregateId : IIdentity;

    IAsyncEnumerable<StoreEvent> ReadAllEventsAsync(
        CancellationToken cancellationToken = default);
}