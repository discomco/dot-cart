using Microsoft.Extensions.DependencyInjection;

namespace Engine.Contract;

public static class Inject
{
    public static IServiceCollection AddRootDocCtors(this IServiceCollection services)
    {
        return services
            .AddRootIDCtor()
            .AddSingleton(Funcs.RootCtor)
            .AddSingleton(Funcs.DetailsCtor);
    }

    public static IServiceCollection AddRootListCtors(this IServiceCollection services)
    {
        return services
            .AddListIDCtor()
            .AddSingleton(Funcs.EngineListItemCtor)
            .AddSingleton(Funcs.ListCtor);
    }


    public static IServiceCollection AddRootIDCtor(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Funcs.RootIDCtor);
    }

    public static IServiceCollection AddListIDCtor(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Funcs.ListIDCtor);
    }
}