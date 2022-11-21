using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IAggregateStore : IDriver, IClose
{
    Task LoadAsync(IAggregate aggregate, CancellationToken cancellationToken = default);
    Task<AppendResult> SaveAsync(IAggregate aggregate, CancellationToken cancellationToken = default);
}

public interface IClose
{
    void Close();
}

public interface ICloseAsync
{
    Task CloseAsync(bool allowCommandsToComplete);
}

public interface IEventStore : IAggregateStore
{
    Task<AppendResult> AppendEventsAsync(IID ID, IEnumerable<IEvt> events,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<IEvt>> ReadEventsAsync(IID ID, CancellationToken cancellationToken = default);
}