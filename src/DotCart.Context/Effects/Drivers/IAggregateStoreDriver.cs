using DotCart.Context.Behaviors;
using DotCart.Contract.Schemas;

namespace DotCart.Context.Effects.Drivers;

public interface IAggregateStoreDriver : IDriver, IClose
{
    Task LoadAsync(IAggregate aggregate, CancellationToken cancellationToken = default);
    Task<AppendResult> SaveAsync(IAggregate aggregate, CancellationToken cancellationToken = default);
}

public interface IClose
{
    void Close();
}

public interface IEventStoreDriver : IAggregateStoreDriver
{
    Task<IEnumerable<IEvt>> ReadEventsAsync(IID ID, CancellationToken cancellationToken = default);

    Task<AppendResult> AppendEventsAsync(IID ID, IEnumerable<IEvt> events,
        CancellationToken cancellationToken = default);
}