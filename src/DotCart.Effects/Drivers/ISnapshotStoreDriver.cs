using DotCart.Behavior;
using DotCart.Schema;

namespace DotCart.Effects.Drivers;

public interface ISnapshotStoreDriver : IClose
{
    Task<Snapshot> SaveSnapshot(IAggregate aggregate);
    Task<Snapshot> GetSnapshot(string aggregateId);
}