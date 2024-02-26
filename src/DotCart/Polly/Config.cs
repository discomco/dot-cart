using DotCart.Core;

namespace DotCart.Polly;

public static class Config
{
    public static int MaxRetries = Convert.ToInt32(DotEnv.Get(EnVars.POLLY_RETRIES) ?? "100");
}