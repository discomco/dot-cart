using DotCart.Behavior;
using DotCart.Schema;

namespace DotCart.Effects;

public interface ISnapshotStore : IClose
{
    Task<Snapshot> SaveSnapshot(IAggregate aggregate);
    Task<Snapshot> GetSnapshot(string aggregateId);
}

