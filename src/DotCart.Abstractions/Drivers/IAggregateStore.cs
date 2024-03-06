using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IAggregateStore : IDriverB, IClose
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

public interface IEventStore
    : IAggregateStore
{
    Task<AppendResult> AppendEventsAsync(IID ID, IEnumerable<IEvtB> events,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<IEvtB>> ReadEventsAsync(IID ID, CancellationToken cancellationToken = default);
}