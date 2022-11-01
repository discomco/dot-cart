namespace DotCart.Drivers.NATS;

public static class Config
{
    public static string User = DotEnv.Get(EnVars.NATS_USER) ?? "a";
    public static string Password = DotEnv.Get(EnVars.NATS_PASSWORD) ?? "a";
    public static string Uri = DotEnv.Get(EnVars.NATS_URI) ?? "nats://nats:4222";
}