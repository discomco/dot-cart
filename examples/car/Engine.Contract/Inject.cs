using Microsoft.Extensions.DependencyInjection;

namespace Engine.Contract;

public static class Inject
{
    public static IServiceCollection AddRootDocCtors(this IServiceCollection services)
    {
        return services
            .AddRootIDCtor()
            .AddSingleton(Schema.RootCtor)
            .AddSingleton(Schema.DetailsCtor);
    }

    public static IServiceCollection AddRootListCtors(this IServiceCollection services)
    {
        return services
            .AddListIDCtor()
            .AddSingleton(Schema.EngineListItemCtor)
            .AddSingleton(Schema.ListCtor);
    }


    public static IServiceCollection AddRootIDCtor(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Schema.RootIDCtor);
    }

    public static IServiceCollection AddListIDCtor(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Schema.ListIDCtor);
    }
}