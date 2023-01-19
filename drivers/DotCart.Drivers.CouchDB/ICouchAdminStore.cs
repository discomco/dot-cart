using System.Threading;
using System.Threading.Tasks;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Defaults.CouchDb;

namespace DotCart.Drivers.CouchDB;

public interface ICouchAdminStore<TDbInfo> 
    : IAdminStore<TDbInfo> 
    where TDbInfo : ICouchDbInfoB
{
    Task<Feedback> OpenDbAsync(string filterDoc = "", CancellationToken cancellationToken = default);
    Task<Feedback> DbExistsAsync(CancellationToken cancellationToken = default);
    Task<Feedback> CreateDbAsync(CancellationToken cancellationToken = default);
    Task<Feedback> ReplicateAsync(string filterDoc = "", CancellationToken cancellationToken = default);
    Task<Feedback> CompactAsync();
    Task<Feedback> DeleteAsync();
    Task<Feedback> InitializeAsync(string filterDoc = "");
    Task<bool> ClearReplication(string id);
    Task<bool> ReplicationExists(string id);
}