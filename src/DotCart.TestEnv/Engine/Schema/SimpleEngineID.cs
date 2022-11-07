using DotCart.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestEnv.Engine.Schema;


public static partial class Inject
{
    public static IServiceCollection AddEngineIDCtor(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => SimpleEngineID.Ctor);
    }
}

public record SimpleEngineID: SimpleID
{

    public static NewSimpleID<SimpleEngineID> Ctor => () => new SimpleEngineID();
    public SimpleEngineID(string value = "") : base(Constants.EngineIDPrefix, value)
    {
    }

    public static SimpleEngineID New()
    {
        return new SimpleEngineID();
    }
}



