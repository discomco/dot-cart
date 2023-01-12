using DotCart.Core;

namespace DotCart.Drivers.OTel;

public static class Config
{
    public static string JaegerUri = DotEnv.Get(EnVars.DOTCART_JAEGER_URI) ?? "http://jaeger:4317";
}