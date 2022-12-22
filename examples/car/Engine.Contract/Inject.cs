using Microsoft.Extensions.DependencyInjection;

namespace Engine.Contract;

public static class Inject
{
    public static IServiceCollection AddRootDocCtor(this IServiceCollection services)
    {
        return services
            .AddRootIDCtor()
            .AddSingleton(Schema.RootCtor);
    }

    public static IServiceCollection AddRootListCtor(this IServiceCollection services)
    {
        return services
            .AddListIDCtor()
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