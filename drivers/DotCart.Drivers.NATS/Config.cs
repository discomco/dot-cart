using DotCart.Core;

namespace DotCart.Drivers.NATS;

public static class Config
{
    public static readonly string User = DotEnv.Get(EnVars.NATS_USER) ?? "a";
    public static readonly string Password = DotEnv.Get(EnVars.NATS_PASSWORD) ?? "a";
    public static readonly string Uri = DotEnv.Get(EnVars.NATS_URI) ?? "nats://nats:4222";
}