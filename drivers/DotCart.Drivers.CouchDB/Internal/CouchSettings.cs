using System.Text;

namespace DotCart.Drivers.CouchDB.Internal;

public static class CouchConstants
{
    public const string DefaultHost = "localhost";
    public const int DefaultPort = 5984;
}

public record CouchSettings(
    string UserName,
    string Password,
    string Host,
    int Port,
    string DatabasePrefix)
{
    public string EncodedCredentials =>
        "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(UserName + ":" + Password));

    private string Password { get; } = Password;
    private string UserName { get; } = UserName;
    public int Port { get; set; } = Port;
    public string Host { get; set; } = Host;
    public string DatabasePrefix { get; set; } = DatabasePrefix;
    public string ServerName => Host + ":" + Port;

    public static CouchSettings New(string usr, string pwd,
        string hst = CouchConstants.DefaultHost,
        int prt = CouchConstants.DefaultPort,
        string _db = "")
    {
        return new CouchSettings(usr, pwd, hst, prt, _db);
    }
}