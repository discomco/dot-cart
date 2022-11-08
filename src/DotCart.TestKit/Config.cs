namespace DotCart.TestKit;

public static class Config
{
    public static bool IsCiCD = Convert.ToBoolean(DotEnv.Get(EnVars.IS_CICD));
}