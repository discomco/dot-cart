namespace DotCart.Drivers.CouchDB.Internal.Interfaces;

public interface ICouchServer
{
    CouchSettings Settings { get; }
    void CreateDatabase(string name);
    void DeleteAllDatabases();
    void DeleteDatabase(string name);
    void DeleteDatabases(string regExp);
    T GetDatabase<T>(string name) where T : ICouchDatabase, new();
    ICouchDatabase GetDatabase(string name);
    T GetDatabase<T>() where T : ICouchDatabase, new();
    IEnumerable<string> GetDatabaseNames();
    T GetExistingDatabase<T>() where T : ICouchDatabase, new();
    T GetExistingDatabase<T>(string name) where T : ICouchDatabase, new();
    ICouchDatabase GetNewDatabase(string name);
    T GetNewDatabase<T>(string name) where T : ICouchDatabase, new();
    bool HasDatabase(string name);
    ICouchRequest Request();
}