using DotCart.Behavior;
using DotCart.Effects.Drivers;
using DotCart.Schema;

namespace DotCart.Effects;

public interface ISnapshotStoreDriver : IClose
{
    Task<Snapshot> SaveSnapshot(IAggregate aggregate);
    Task<Snapshot> GetSnapshot(string aggregateId);
}