using DotCart.Abstractions.Behavior;

namespace DotCart.Abstractions.Drivers;

public interface ISnapshotStoreDriver : IClose
{
    Task<Snapshot> SaveSnapshot(IAggregate aggregate);
    Task<Snapshot> GetSnapshot(string aggregateId);
}