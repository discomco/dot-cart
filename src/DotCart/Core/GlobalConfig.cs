﻿namespace DotCart.Core;

public static class GlobalConfig
{
    public static bool IsTest => Convert.ToBoolean(DotEnv.Get(EnVars.DOTCART_IS_TEST));
}

public static class EnVars
{
    public const string DOTCART_IS_TEST = "DOTCART_IS_TEST";
    public const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";
}