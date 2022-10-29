namespace DotCart.Drivers.ElasticSearch;

public static class Config
{
    public static string Url = Environment.GetEnvironmentVariable(EnVars.DOTCART_ELASTIC_URL) ?? "http://es-svc:9200";
    public static string TimeOutMin = Environment.GetEnvironmentVariable(EnVars.DOTCART_ELASTIC_TIMEOUT_MINS) ?? "2";
}