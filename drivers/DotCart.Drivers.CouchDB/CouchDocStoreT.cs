using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.Defaults.CouchDb;
using MyCouch;
using MyCouch.Requests;
using Newtonsoft.Json.Linq;
using Serilog;

namespace DotCart.Drivers.CouchDB;

public class CouchDocStoreT<TDbInfo, TDoc, TID>
    : CouchAdminStoreT<TDbInfo>, ICouchDocStoreT<TDbInfo, TDoc, TID>
    where TDoc : IState
    where TID : IID
    where TDbInfo : ICouchDbInfoB
{
    private readonly IMyCouchClient _client;
    private readonly string _dbName = DbNameAtt.Get<TDbInfo>();
    private readonly IMyCouchFactory _myCouchFactory;
    private readonly IMyCouchServerClient _server;
    private readonly IMyCouchStore _store;

    public CouchDocStoreT(IMyCouchFactory myCouchFactory) : base(myCouchFactory)
    {
        _myCouchFactory = myCouchFactory;
        _client = myCouchFactory.Client(_dbName);
        _server = myCouchFactory.Server();
        _store = myCouchFactory.Store(_dbName);
    }


    public void Dispose()
    {
        if (_server != null) _server.Dispose();
        if (_store != null) _store.Dispose();
        if (_client != null) _client.Dispose();
    }


    public void Close()
    {
    }

    public ValueTask DisposeAsync()
    {
        return new ValueTask(Task.Run(Dispose));
    }

    public Task CloseAsync(bool allowCommandsToComplete)
    {
        return Task.Run(Close);
    }

    public async Task<TDoc> SetAsync(string id, TDoc doc, CancellationToken cancellationToken = default)
    {
        try
        {
            await OpenDbAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            var cDoc = doc.AsCDoc();
            var rsp = await _store.SetAsync(cDoc, cancellationToken)
                .ConfigureAwait(false);
            rsp.Data.Rev = rsp._rev;
            return rsp.Data;
        }
        catch (Exception e)
        {
            Log.Error(AppErrors.Error(e.InnerAndOuter()));
            throw;
        }
    }

    public async Task<TDoc> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var exists = await Exists(id, cancellationToken)
                .ConfigureAwait(false);
            if (!exists)
                return default;
            var doc = await GetByIdAsync(id, cancellationToken)
                .ConfigureAwait(true);
            var delReq = new DeleteDocumentRequest(id, doc.Rev);
            var resp = await _client.Documents
                .DeleteAsync(delReq, cancellationToken)
                .ConfigureAwait(false);
            if (resp.IsSuccess)
                return doc;
        }
        catch (Exception e)
        {
            Log.Error(AppErrors.Error(e.InnerAndOuter()));
            throw;
        }

        return default;
    }

    public async Task<bool> Exists(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var headReq = new HeadDocumentRequest(id);
            var headResp = await _client.Documents
                .HeadAsync(headReq, cancellationToken)
                .ConfigureAwait(false);
            return headResp.IsSuccess;
        }
        catch (Exception e)
        {
            Log.Error(AppErrors.Error(e.InnerAndOuter()));
            throw;
        }
    }

    public async Task<TDoc?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var getReq = new GetDocumentRequest(id);
            var rsp = await _client.Documents.GetAsync(getReq, cancellationToken).ConfigureAwait(false);
            if (rsp.IsSuccess)
            {
                var jObj = JObject.Parse(rsp.Content);
                var doc = jObj["data"].ToObject<TDoc>();
                doc.Rev = rsp.Rev;
                return doc;
            }
        }
        catch (Exception e)
        {
            Log.Error(AppErrors.Error(e.InnerAndOuter()));
            throw;
        }

        return default;
    }

    public async Task<bool> HasData(CancellationToken cancellationToken = default)
    {
        try
        {
            var hasData = await _server
                .Databases.HeadAsync(_dbName, cancellationToken)
                .ConfigureAwait(false);
            return hasData.IsSuccess;
        }
        catch (Exception e)
        {
            Log.Error(AppErrors.Error(e.InnerAndOuter()));
            throw;
        }
    }
}