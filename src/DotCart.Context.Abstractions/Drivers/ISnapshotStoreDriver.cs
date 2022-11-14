namespace DotCart.Context.Abstractions.Drivers;

public interface ISnapshotStoreDriver : IClose
{
    Task<Snapshot> SaveSnapshot(IAggregate aggregate);
    Task<Snapshot> GetSnapshot(string aggregateId);
}