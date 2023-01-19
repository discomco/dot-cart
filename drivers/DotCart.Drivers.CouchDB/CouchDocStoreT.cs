using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.Defaults.CouchDb;
using MyCouch;

namespace DotCart.Drivers.CouchDB;

public class CouchDocStoreT<TDbInfo, TDoc, TID>
    : ICouchDocStoreT<TDbInfo, TDoc, TID>
    where TDoc : IState
    where TID : IID
    where TDbInfo : ICouchDbInfoB
{


    private readonly string _dbName = DbNameAtt.Get<TDbInfo>();
    private readonly IMyCouchFactory _myCouchFactory;
    private readonly IMyCouchClient _client;
    private readonly IMyCouchServerClient _server;
    private readonly IMyCouchStore _store;

    public CouchDocStoreT(IMyCouchFactory myCouchFactory)
    {
        _myCouchFactory = myCouchFactory;
        _client = myCouchFactory.Client(_dbName);
        _server = myCouchFactory.Server();
        _store = myCouchFactory.Store(_dbName);
    }


    public void Dispose()
    {
        if (_server !=null) _server.Dispose();
        if (_store !=null) _store.Dispose();
        if (_client!=null) _client.Dispose();
    }


    public void Close()
    { }

    public ValueTask DisposeAsync()
    {
        return new ValueTask(Task.Run(Dispose));
    }

    public Task CloseAsync(bool allowCommandsToComplete)
    {
        return Task.Run(Close);
    }

    private static CDoc<TDoc> CreateCDoc(TDoc model)
    {
        return new CDoc<TDoc>
        {
            _id = model.Id,
            _rev = model.Prev,
            Data = model,
            TimeStamp = DateTime.UtcNow
        };
    }


    public async Task<TDoc> SetAsync(string id, TDoc doc, CancellationToken cancellationToken = default)
    {
        using var store = _myCouchFactory.Store(DbNameAtt.Get<TDbInfo>());
        var cDoc = CreateCDoc(doc);
        var cRes = await store.StoreAsync(cDoc, cancellationToken);
        return cRes.Data;
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