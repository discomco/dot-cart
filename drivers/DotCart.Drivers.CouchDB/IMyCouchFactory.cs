using MyCouch;

namespace DotCart.Drivers.CouchDB;

public interface IMyCouchFactory
{
    IMyCouchClient Client(string dbName);
    IMyCouchStore Store(string dbName);
    IMyCouchServerClient Server();
}