using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Drivers;
using DotCart.Defaults.CouchDb;

namespace DotCart.Drivers.CouchDB;

public interface ICouchAdminStore<TDbInfo>
    : IAdminStore<TDbInfo>
    where TDbInfo : ICouchDbInfoB
{
    Task<IFeedback> OpenDbAsync(string filterDoc = "", CancellationToken cancellationToken = default);
    Task<IFeedback> DbExistsAsync(CancellationToken cancellationToken = default);
    Task<IFeedback> CreateDbAsync(CancellationToken cancellationToken = default);
    Task<IFeedback> ReplicateAsync(string filterDoc = "", CancellationToken cancellationToken = default);
    Task<IFeedback> CompactAsync();
    Task<IFeedback> DeleteAsync();
    Task<IFeedback> InitializeAsync(string filterDoc = "");
    Task<bool> ClearReplication(string id);
    Task<bool> ReplicationExists(string id);
    Task<IFeedback> ReplicationExistsAsync(string repId, CancellationToken cancellationToken = default);
}