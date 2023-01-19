using System;
using System.Threading;
using System.Threading.Tasks;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.Defaults.CouchDb;
using MyCouch;
using MyCouch.Requests;
using Serilog;

namespace DotCart.Drivers.CouchDB;

public class CouchAdminStoreT<TDbInfo>
    : ICouchAdminStore<TDbInfo>
    where TDbInfo : ICouchDbInfoB
{
    private readonly IMyCouchFactory _myCouchFactory;
    private readonly IMyCouchServerClient _server;
    
    private static readonly string _dbName = DbNameAtt.Get<TDbInfo>().ToLower();
    private static readonly bool _canReplicate = ReplicateAtt.Get<TDbInfo>();
    private static readonly string _upstreamReplicationId = $"up:{_dbName}";
    
    public CouchAdminStoreT(IMyCouchFactory myCouchFactory)
    {
        _myCouchFactory = myCouchFactory;
        _server = myCouchFactory.Server();
    }

    public void Close()
    {
    }

    public void Dispose()
    {
        if (_server != null) _server.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return new ValueTask(Task.Run(Dispose));
    }

    public Task CloseAsync(bool allowCommandsToComplete)
    {
        return Task.CompletedTask;
    }

    public async Task<Feedback> OpenDbAsync(string filterDoc="", CancellationToken cancellationToken = default)
    {
        var res = Feedback.Empty;
        try
        {
            if (!Config.GetDbExists(_dbName))
            {
                var existsFeedback = await DbExistsAsync(cancellationToken)
                    .ConfigureAwait(false);
                if (!existsFeedback.IsSuccess)
                {
                    res = await CreateDbAsync(cancellationToken)
                        .ConfigureAwait(false);
                }
                if (_canReplicate)
                {
                    if (_dbName == CouchConstants.ReplicatorDbName) 
                        return res;
                    res = await InitializeAsync(filterDoc)
                        .ConfigureAwait(false);
                    res = await ReplicateAsync(filterDoc, cancellationToken);
                }
            }
        }
        catch (Exception e)
        {
            Log.Fatal(AppErrors.Error($"OpenDb for {_dbName} => {e.InnerAndOuter()}"));
            res.SetError(e.AsError());
        }
        finally
        {
            await Log.CloseAndFlushAsync().ConfigureAwait(false);
        }
        return res;
    }

    public async Task<Feedback> ReplicationExistsAsync(string repId, CancellationToken cancellationToken = default)
    {
        var fbk = Feedback.Empty;
        try
        {
            using (var replicationStore = _myCouchFactory.Client(CouchConstants.ReplicatorDbName))
            {
                var rsp = await replicationStore.Documents.HeadAsync(repId, cancellationToken: cancellationToken);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return fbk;
    }


    public async Task<Feedback> DbExistsAsync(CancellationToken cancellationToken = default)
    {
        var fbk = Feedback.Empty;
        try
        {
            var req = new HeadDatabaseRequest(_dbName);
            var rsp = await _server.Databases.HeadAsync(req, cancellationToken);
            if (rsp.IsSuccess)
                return fbk;
            fbk.SetError(rsp.AsError());
        }
        catch (Exception e)
        {
            Log.Error(AppErrors.Error(e.InnerAndOuter()));
            fbk.SetError(e.AsError());
        }

        return fbk;
    }

    public async Task<Feedback> CreateDbAsync(CancellationToken cancellationToken = default)
    {
        var fbk = Feedback.Empty;
        try
        {
            var exists = await DbExistsAsync(cancellationToken)
                .ConfigureAwait(false);
            if (!exists.IsSuccess)
            {
                Log.Information(AppVerbs.Creating($"database: {_dbName}"));
                var putRequest = new PutDatabaseRequest(_dbName);
                var crr = await _server.Databases.PutAsync(putRequest, cancellationToken).ConfigureAwait(false);
                if (crr.IsSuccess)
                {
                    Config.SetDbExists(_dbName);
                    return fbk;
                }
                fbk.SetError(crr.AsError());
            }
        }
        catch (Exception e)
        {
            Log.Error(AppErrors.Error(e.InnerAndOuter()));
            fbk.SetError(e.AsError());
        }

        return fbk;
    }

    public async Task<Feedback> ReplicateAsync(string filterDoc = "", CancellationToken cancellationToken=default)
    {
        var fbk = Feedback.Empty;
        try
        {
            if (!_canReplicate)
            {
                var warning = $"{_dbName} is not marked for replication";
                fbk.AddWarning(warning);
                Log.Warning(AppVerbs.Warning(warning));
                return fbk;
            }
            if (_dbName.ToLower() == CouchConstants.ReplicatorDbName) return null;
            if (Config.LocalSource == Config.ClusterSource) return null;
            var existsFbk = await ReplicationExistsAsync(_upstreamReplicationId, cancellationToken)
                .ConfigureAwait(false);
            if (existsFbk.IsSuccess)
            {
                var warning = $"Replication [{_upstreamReplicationId}] already exists";
                fbk.AddWarning(AppVerbs.Warning(warning));
                return fbk;
            }
            var source = $"{Config.LocalhostUrl}/{_dbName}";
            var target = $"{Config.ClusterUrl}/{_dbName}";
            var request = new ReplicateDatabaseRequest(_upstreamReplicationId, source, target)
            {
                Continuous = true,
                CreateTarget = true,
                Filter = filterDoc
            };
            var res = await _server.Replicator.ReplicateAsync(request);
            if (!res.IsSuccess)
            {
                var msg = $"Replication [{_upstreamReplicationId}] could not be registered. Reason: {res.Reason}";
                throw new Exception(msg);
            }
            var info = $"Replication [{_upstreamReplicationId}] registered successfully.";
            fbk.AddInfo(info);
            return fbk;
        }
        catch (Exception e)
        {
            Log.Error(AppErrors.Error(e.InnerAndOuter()));
            fbk.SetError(e.AsError());
        }
        return fbk;
    }


    public Task<Feedback> CompactAsync()
    {
        throw new NotImplementedException();
        // using var client = new MyCouchServerClient(StoreConfig.LocalServer);
        // var request = new CompactDatabaseRequest(DbName);
        // var res = await client.Databases.CompactAsync(request);
        // return res.ToXResponse();
    }

    public Task<Feedback> DeleteAsync()
    {
        throw new NotImplementedException();
        // using var client = new MyCouchServerClient(StoreConfig.LocalServer);
        // var res = await client.Databases.DeleteAsync(DbName);
        // return res.ToXResponse();
    }

    public Task<Feedback> InitializeAsync(string filterDoc = "")
    {
        throw new NotImplementedException();
        // if (DbName.ToLower() == "_replicator") return null;
        // if (StoreConfig.LocalSource == StoreConfig.ClusterSource) return null;
        // using var client = new MyCouchServerClient(StoreConfig.LocalServer);
        // var repId = $"down:{DbName}";
        // if (await ReplicationExists(repId))
        // {
        //     var resp = new XResponse
        //     {
        //         Id = repId,
        //         DbName = DbName,
        //         IsSuccess = false,
        //         Reason = "Replication DbExists"
        //     };
        //     return resp;
        // }
        //
        // //await ClearReplication(repId);
        // var source = $"{StoreConfig.ClusterServer}/{DbName}";
        // //var target = $"{StoreConfig.LocalServer}/{DbName}";
        // var target = $"{StoreConfig.StrictLocalhostServer}/{DbName}";
        // var request = new ReplicateDatabaseRequest(repId, source, target)
        // {
        //     Continuous = true,
        //     CreateTarget = true,
        //     Filter = filterDoc
        // };
        // var res = await client.Replicator.ReplicateAsync(request);
        // return res.ToXResponse();
    }

    public Task<bool> ClearReplication(string id)
    {
        throw new NotImplementedException();
        // return await new StoreBuilder<TReadModel>()
        //     .BuildAdmin("_replicator")
        //     .DeleteAsync(id);
    }

    public Task<bool> ReplicationExists(string id)
    {
        throw new NotImplementedException();
        // var res = await new StoreBuilder<TReadModel>()
        //     .BuildAdmin("_replicator")
        //     .DocExists(id);
        // return res;
    }
}