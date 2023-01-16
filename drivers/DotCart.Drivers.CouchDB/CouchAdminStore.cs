using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Defaults.CouchDb;

namespace DotCart.Drivers.CouchDB;

public class CouchAdminStore<TDbInfo> : ICouchAdminStore<TDbInfo> 
    where TDbInfo : ICouchDbInfoB
{
    public void Close()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public Task CloseAsync(bool allowCommandsToComplete)
    {
        throw new NotImplementedException();
    }

    public Task<Feedback> ReplicateAsync(string filterDoc = "")
    {
        throw new NotImplementedException();
    }

    public Task<Feedback> CompactAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Feedback> DeleteAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Feedback> InitializeAsync(string filterDoc = "")
    {
        throw new NotImplementedException();
    }

    public Task<bool> ClearReplication(string id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ReplicationExists(string id)
    {
        throw new NotImplementedException();
    }
}

public interface ICouchAdminStore<TDbInfo> : IAdminStore<TDbInfo> 
    where TDbInfo : ICouchDbInfoB
{
    Task<Feedback> ReplicateAsync(string filterDoc = "");
    Task<Feedback> CompactAsync();
    Task<Feedback> DeleteAsync();
    Task<Feedback> InitializeAsync(string filterDoc = "");
    Task<bool> ClearReplication(string id);
    Task<bool> ReplicationExists(string id);
}