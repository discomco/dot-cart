using DotCart.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestEnv.Engine.Schema;


public static partial class Inject
{
    public static IServiceCollection AddEngineIDCtor(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => EngineID.Ctor);
    }
}

[IDPrefix(Constants.EngineIDPrefix)]
public record EngineID: ID
{

    public static NewID<EngineID> Ctor => () => new EngineID();
    public EngineID(string value = "") : base(Constants.EngineIDPrefix, value)
    {
    }

    public static EngineID New()
    {
        return new EngineID();
    }
}



