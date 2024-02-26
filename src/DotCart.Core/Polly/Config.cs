namespace DotCart.Core.Polly;

public static class Config
{
    public static int MaxRetries = Convert.ToInt32(DotEnv.Get(EnVars.POLLY_RETRIES) ?? "100");
}