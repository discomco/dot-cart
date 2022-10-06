namespace DotCart.Schema;

public enum IDCreationPolicy
{
    Strict = 1,
    Liberal = 2
}

public static class Config
{
    public static IDCreationPolicy IdCreationPolicy =
        (IDCreationPolicy)Convert.ToInt32(DotEnv.Get(EnVars.DOTCART_ID_CREATION_POLICY) ?? "1");
}

public static class EnVars
{
    public const string DOTCART_ID_CREATION_POLICY = "DEC_ID_CREATION_POLICY";
}