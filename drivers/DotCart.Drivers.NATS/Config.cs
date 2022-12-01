using DotCart.Core;

namespace DotCart.Drivers.NATS;

public static class Config
{
    public static readonly string User = DotEnv.Get(EnVars.NATS_USR) ?? "a";
    public static readonly string Password = DotEnv.Get(EnVars.NATS_PWD) ?? "a";
    public static readonly string Uri = DotEnv.Get(EnVars.NATS_URI) ?? "nats://nats:4222";
}