using DotCart.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestEnv.Engine.Schema;


public static partial class Inject
{
    public static IServiceCollection AddTypedEngineIDCtor(this IServiceCollection services)
    {
        return services
            .AddSingleton(TypedEngineID.Ctor);
    }
}


public static class Constants
{
    public const string EngineIDPrefix = "engine";
}

[IDPrefix(Constants.EngineIDPrefix)]
public record TypedEngineID : TypedID<TypedEngineID>
{
    public static NewTypedID<TypedEngineID> Ctor => () => New;
    public TypedEngineID(string value) : base(value)
    {
    }
}