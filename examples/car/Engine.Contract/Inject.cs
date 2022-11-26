using Microsoft.Extensions.DependencyInjection;

namespace Engine.Contract;

public static class Inject
{
    public static IServiceCollection AddIDCtor(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Schema.IDCtor);
    }

    public static IServiceCollection AddEngineContract(this IServiceCollection services)
    {
        return services
            .AddIDCtor();
    }
}