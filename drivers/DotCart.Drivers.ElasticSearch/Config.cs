using DotCart.Core;

namespace DotCart.Drivers.ElasticSearch;

public static class Config
{
    public static string Url = DotEnv.Get(EnVars.DOTCART_ELASTIC_URL) ?? "http://elastic:9200";
    public static string TimeOutMin = DotEnv.Get(EnVars.DOTCART_ELASTIC_TIMEOUT_MINS) ?? "2";
}