using DotCart.Context.Behaviors;
using DotCart.Context.Schemas;

namespace DotCart.Context.Effects.Drivers;

public interface ISnapshotStoreDriver : IClose
{
    Task<Snapshot> SaveSnapshot(IAggregate aggregate);
    Task<Snapshot> GetSnapshot(string aggregateId);
}