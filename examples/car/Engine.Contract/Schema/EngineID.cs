using DotCart.Abstractions.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Contract.Schema;

public static class Inject
{
    public static IServiceCollection AddModelIDCtor(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => EngineID.Ctor);
    }
}

[IDPrefix(Constants.EngineIDPrefix)]
public record EngineID : ID
{
    public EngineID(string value = "") : base(Constants.EngineIDPrefix, value)
    {
    }

    public static NewID<EngineID> Ctor => () => new EngineID();

    public static EngineID New()
    {
        return new EngineID();
    }
}