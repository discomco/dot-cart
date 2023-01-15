using Ardalis.GuardClauses;
using CouchDB.Driver;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.Defaults.CouchDb;
using DotCart.Drivers.CouchDB.Internal.Interfaces;
using Serilog;

namespace DotCart.Drivers.CouchDB;

public class CouchStore<TDbInfo, TDoc, TId>
    : CouchStoreT<TDbInfo, TDoc, TId>
    where TId : IID
    where TDoc : IState
    where TDbInfo : ICouchDbInfoB
{
    private readonly ICouchClient _couchClient;
    private readonly ICouchServer _couchServer;

    public CouchStore(ICouchClient client, ICouchServer srv)
        : base(client, srv)
    {
    }


    public override async Task<TDoc> AddOrUpdateAsync(string id, TDoc entity, bool batch = false,
        bool withConflicts = false, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entity, nameof(entity));
        Guard.Against.NullOrWhiteSpace(entity.Id, nameof(entity));
        TDoc res;
        var couchDoc = CreateCDoc(entity.Id, entity, entity.Prev);
        try
        {
            var oldDoc = await Db.FindAsync(couchDoc.Id, withConflicts, cancellationToken);
            if (oldDoc != null)
                if (oldDoc.Rev != entity.Prev)
                    couchDoc.Rev = oldDoc.Rev;
            couchDoc = await Db.AddOrUpdateAsync(couchDoc, batch, cancellationToken);
            res = couchDoc.Data;
            res.Prev = couchDoc.Rev;
        }
        catch (Exception e)
        {
            Log.Error(AppErrors.Error(e.InnerAndOuter()));
            throw;
        }

        return res;
    }

    public override Task<TDoc> SetAsync(string id, TDoc doc, CancellationToken cancellationToken = default)
    {
        return AddOrUpdateAsync(id, doc, cancellationToken: cancellationToken);
    }

    public override async Task<bool> Exists(string id, CancellationToken cancellationToken = default)
    {
        var res = await Db.FindAsync(id, false, cancellationToken).ConfigureAwait(false);
        return res != null;
    }

    public override async Task<bool> HasData(CancellationToken cancellationToken = default)
    {
        return Db.Database.Any();
    }
}