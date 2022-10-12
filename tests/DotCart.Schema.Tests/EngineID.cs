namespace DotCart.Schema.Tests;

public static class Constants
{
    public const string IdPrefix = "engine";
}


[IDPrefix(Constants.IdPrefix)]
public record EngineID : ID<EngineID>
{
    public EngineID(string value) : base(value)
    {}
}