using DotCart.Core;

namespace DotCart.Drivers.EventStoreDB;

public static class Config
{
    public static string Uri
        => DotEnv.Get(EnVars.EVENTSTORE_URI) ?? Defaults.Uri;

    public static string UserName
        => DotEnv.Get(EnVars.EVENTSTORE_USER) ?? Defaults.UserName;

    public static string Password
        => DotEnv.Get(EnVars.EVENTSTORE_PWD) ?? Defaults.Password;

    /// <summary>
    ///     If true, make sure you have GRPC_DEFAULT_SSL_ROOTS_FILE_PATH environment variable set
    /// </summary>
    public static bool UseTls
        => Convert.ToBoolean(DotEnv.Get(EnVars.EVENTSTORE_USE_TLS) ?? Defaults.False);

    public static bool Insecure
        => Convert.ToBoolean(DotEnv.Get(EnVars.EVENTSTORE_INSECURE) ?? Defaults.True);

    private static class Defaults
    {
        public const string Uri = "esdb+discover://127.0.0.1:2113?tls=false";

        // public const string Uri = "tcp://es.local:2113";
        // public const string Uri = "esdb://es.local:2113?tls=false";
        public const string UserName = "admin";
        public const string Password = "changeit";
        public const string False = "false";
        public const string True = "true";
    }
}