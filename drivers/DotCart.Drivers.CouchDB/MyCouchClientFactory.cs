using MyCouch;

namespace DotCart.Drivers.CouchDB;

public class MyCouchClientFactory : IMyCouchFactory
{
    private readonly ServerConnectionInfo _serverConnectionInfo;

    public MyCouchClientFactory(Func<ServerConnectionInfo> serverConnectionInfo)
    {
        _serverConnectionInfo = serverConnectionInfo();
    }

    public IMyCouchClient Client(string dbName)
    {
        var info = new DbConnectionInfo(_serverConnectionInfo.Address, dbName)
        {
            BasicAuth = _serverConnectionInfo.BasicAuth
        };
        return new MyCouchClient(info);
    }

    public IMyCouchServerClient Server()
    {
        return new MyCouchServerClient(_serverConnectionInfo);
    }

    public IMyCouchStore Store(string dbName)
    {
        return new MyCouchStore(Client(dbName));
    }
}