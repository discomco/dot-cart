using DotCart.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestEnv.Engine.Schema;


public static partial class Inject
{
    public static IServiceCollection AddIDCtor(this IServiceCollection services)
    {
        return services
            .AddSingleton(EngineID.Ctor);
    }
}


public static class Constants
{
    public const string EngineIDPrefix = "engine";
}

[IDPrefix(Constants.EngineIDPrefix)]
public record EngineID : ID<EngineID>
{
    public static NewID<EngineID> Ctor => () => New;
    public EngineID(string value) : base(value)
    {
    }
}