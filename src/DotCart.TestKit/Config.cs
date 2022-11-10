namespace DotCart.TestKit;

public static class Config
{
    public static readonly bool IsPipeline = Convert.ToBoolean(DotEnv.Get(EnVars.IS_CICD));
}