using Engine.Context;
using Engine.Context.Common.Effects;
using Engine.Context.Initialize;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.TestConsole;

public static class Inject
{
    public static IServiceCollection BuildTestApp(this IServiceCollection services)
    {
        return services
            // .AddConfiguredESDBClients()
            // .AddESDBEventStoreDriver()
            .AddESDBEngineEventFeeder()
            .AddESDBInfra<Spoke>()
            .AddInitializeSpoke();
        ;
    }
}