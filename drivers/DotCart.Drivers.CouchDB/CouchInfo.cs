using MyCouch;

namespace DotCart.Drivers.CouchDB;

public record ReplicationCouchInfo(CouchInfo LocalInfo, CouchInfo RemoteInfo)
{
    public CouchInfo LocalInfo { get; } = LocalInfo;
    public CouchInfo RemoteInfo { get; } = RemoteInfo;
}

public record CouchInfo(ServerConnectionInfo ServerInfo, DbConnectionInfo DbInfo)
{
    public ServerConnectionInfo ServerInfo { get; } = ServerInfo;
    public DbConnectionInfo DbInfo { get; } = DbInfo;
}