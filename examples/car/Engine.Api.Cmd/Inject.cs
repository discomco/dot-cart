using Engine.Context;

namespace Engine.Api.Cmd;

public static class Inject
{
    public static IServiceCollection AddEngineApp(this IServiceCollection services)
    {
        return services
            .AddEngineContext();
        ;
        ;
        // .AddStartSpoke()
        // .AddChangeRpmSpoke();
    }
}