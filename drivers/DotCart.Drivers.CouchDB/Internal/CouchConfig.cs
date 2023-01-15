using DotCart.Core;

namespace DotCart.Drivers.CouchDB.Internal;

public static class CouchConfig
{
    public static string ClusterHost
        => DotEnv.Get(EnVars.COUCH_CLUSTER_HOST) ?? "couch.cluster";

    public static int ClusterPort
        => Convert.ToInt32(DotEnv.Get(EnVars.COUCH_CLUSTER_PORT) ?? "80");

    public static string ClusterUser
        => DotEnv.Get(EnVars.COUCH_CLUSTER_USER) ?? "root";

    public static string ClusterPwd
        => DotEnv.Get(EnVars.COUCH_CLUSTER_PWD) ?? "toorc";

    public static string ClusterProtocol
        => DotEnv.Get(EnVars.COUCH_CLUSTER_PROTOCOL) ?? "http";

    public static string LocalHost
        => DotEnv.Get(EnVars.COUCH_LOCAL_HOST) ?? "localhost";

    public static string LocalProtocol
        => DotEnv.Get(EnVars.COUCH_LOCAL_PROTOCOL) ?? "http";

    public static int LocalPort =>
        Convert.ToInt32(DotEnv.Get(EnVars.COUCH_LOCAL_PORT) ?? "5984");

    public static string LocalUser => DotEnv.Get(EnVars.COUCH_LOCAL_USER) ?? "root";
    public static string LocalPwd => DotEnv.Get(EnVars.COUCH_LOCAL_PWD) ?? "X3!k0n";


    public static string ClusterServer =>
        $"{ClusterProtocol}://{ClusterUser}:{ClusterPwd}@{ClusterHost}:{ClusterPort}";

    public static string LocalServer =>
        $"{LocalProtocol}://{LocalUser}:{LocalPwd}@{LocalHost}:{LocalPort}";

    public static string ClusterSource =>
        $"{ClusterProtocol}://{ClusterHost}:{ClusterPort}";

    public static string LocalSource =>
        $"{LocalProtocol}://{LocalHost}:{LocalPort}";

    public static bool CanReplicate =>
        Convert.ToBoolean(DotEnv.Get(EnVars.COUCH_CAN_REPLICATE) ?? "true");

    public static void BuildEnvironment()
    {
        Environment.SetEnvironmentVariable(EnVars.COUCH_LOCAL_HOST, LocalHost);
        Environment.SetEnvironmentVariable(EnVars.COUCH_LOCAL_PROTOCOL, LocalProtocol);
        Environment.SetEnvironmentVariable(EnVars.COUCH_LOCAL_PORT, Convert.ToString(LocalPort));
        Environment.SetEnvironmentVariable(EnVars.COUCH_LOCAL_USER, LocalUser);
        Environment.SetEnvironmentVariable(EnVars.COUCH_LOCAL_PWD, LocalPwd);
        Environment.SetEnvironmentVariable(EnVars.COUCH_CAN_REPLICATE, Convert.ToString(CanReplicate));
        Environment.SetEnvironmentVariable(EnVars.COUCH_CLUSTER_HOST, ClusterHost);
        Environment.SetEnvironmentVariable(EnVars.COUCH_CLUSTER_PROTOCOL, ClusterProtocol);
        Environment.SetEnvironmentVariable(EnVars.COUCH_CLUSTER_PORT, Convert.ToString(ClusterPort));
        Environment.SetEnvironmentVariable(EnVars.COUCH_CLUSTER_USER, ClusterUser);
        Environment.SetEnvironmentVariable(EnVars.COUCH_CLUSTER_PWD, ClusterPwd);
    }

    public static bool GetDbExists(string dbName)
    {
        return !string.IsNullOrWhiteSpace(DotEnv.Get($"{dbName}_EXISTS"));
    }


    public static void SetDbExists(string dbName)
    {
        Environment.SetEnvironmentVariable($"{dbName}_EXISTS", "true");
    }
}