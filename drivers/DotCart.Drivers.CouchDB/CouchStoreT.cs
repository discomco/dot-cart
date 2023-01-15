using Ardalis.GuardClauses;
using CouchDB.Driver;
using CouchDB.Driver.Indexes;
using CouchDB.Driver.Types;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.Defaults.CouchDb;
using DotCart.Drivers.CouchDB.Internal.Interfaces;
using Serilog;

namespace DotCart.Drivers.CouchDB;

public abstract class CouchStoreT<TDbInfo, TDoc, TID>
    : ICouchDBStore<TDbInfo, TDoc, TID>
    where TDoc : IState
    where TID : IID
    where TDbInfo : ICouchDbInfoB
{
    private readonly ICouchClient _clt;
    private readonly ICouchServer _srv;

    private ICouchDatabase<CDoc<TDoc>> _cachedDb;

    protected CouchStoreT(ICouchClient client, ICouchServer srv)
    {
        _clt = client;
        _srv = srv;
    }


    protected ICouchDatabase<CouchUser> UsersDb
        => GetUsersDb();

    protected ICouchDatabase<CDoc<TDoc>> Db
        => GetDb().Result;

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public abstract Task<bool> Exists(string id, CancellationToken cancellationToken = default);

    public async Task<TDoc> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        Guard.Against.NullOrWhiteSpace(id, nameof(id));
        var doc = await Db
            .FindAsync(id, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        if (doc == null) return default;
        var res = doc.Data;
        res.Prev = doc.Rev;
        return res;
    }

    public abstract Task<bool> HasData(CancellationToken cancellationToken = default);


    public abstract Task<TDoc> SetAsync(string id, TDoc doc, CancellationToken cancellationToken = default);

    public async Task<TDoc> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            Guard.Against.NullOrWhiteSpace(id, nameof(id));
            var res = await GetByIdAsync(id, cancellationToken)
                .ConfigureAwait(false);
            if (res == null)
                return default;
            var doc = CreateCDoc(id, res, res.Prev);
            await Db.RemoveAsync(doc, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            res.Prev = string.Empty;
            return res;
        }
        catch (Exception e)
        {
            Log.Error(AppErrors.Error(e.InnerAndOuter()));
            return default;
        }
    }

    public void Close()
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


    public abstract Task<TDoc> AddOrUpdateAsync(string id, TDoc entity, bool batch = false,
        bool withConflicts = false,
        CancellationToken cancellationToken = default);


    public async Task<IEnumerable<TDoc>> RetrieveRecent(int pageNumber, int pageSize)
    {
        Guard.Against.SmallerThanOne(pageNumber, nameof(pageNumber));
        Guard.Against.SmallerThanOne(pageSize, nameof(pageSize));
        await InitializeIndexesAsync();
        return Db.OrderByDescending(d => d.TimeStamp)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .Select(x => x.Data)
            .ToArray();
    }


    protected async Task InitializeIndexesAsync()
    {
        var indexes = await Db.GetIndexesAsync().ConfigureAwait(false);
        if (indexes.Exists(x => x.Name == "recent" && x.DesignDocument == "default")) return;
        await Db.CreateIndexAsync("recent",
            builder => { builder.IndexByDescending(d => d.TimeStamp); },
            new IndexOptions
            {
                DesignDocument = "default",
                Partitioned = false
            }).ConfigureAwait(false);
    }


    private async Task<ICouchDatabase<CDoc<TDoc>>> GetDb()
    {
        var dbName = DbNameAtt.Get<TDbInfo>();
        if (_cachedDb != null)
            return _cachedDb;
        _cachedDb = await _clt.GetOrCreateDatabaseAsync<CDoc<TDoc>>(dbName);
        return _cachedDb;
    }


    protected CDoc<TDoc> CreateCDoc(string id, TDoc model, string prev = default)
    {
        Guard.Against.Null(model, nameof(model));
        Guard.Against.NullOrWhiteSpace(id, nameof(id));
        return new CDoc<TDoc>
        {
            Id = id,
            Rev = prev,
            Data = model,
            TimeStamp = DateTime.UtcNow
        };
    }


    protected ICouchDatabase<CouchUser> GetUsersDb()
    {
        try
        {
            return _clt.GetUsersDatabase();
        }
        catch (Exception e)
        {
            Log.Error(AppErrors.Error(e.InnerAndOuter()));
            throw;
        }
    }
}