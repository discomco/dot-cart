using DotCart.Abstractions.Schema;
using DotCart.Defaults.Elastic;
using Nest;

namespace DotCart.Drivers.ElasticSearch;

public class ElasticStoreT<TDoc> : IElasticStore<TDoc>
    where TDoc : class, IState
{
    private readonly IElasticClient _client;

    public ElasticStoreT(IElasticClient client)
    {
        _client = client;
    }

    public void Close()
    {
    }

    public void Dispose()
    {
    }

    public ValueTask DisposeAsync()
    {
        return new ValueTask(Task.CompletedTask);
    }

    public Task CloseAsync(bool allowCommandsToComplete)
    {
        return Task.CompletedTask;
    }

    public Task<TDoc> SetAsync(string id, TDoc doc, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TDoc> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Exists(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TDoc?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasData(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}