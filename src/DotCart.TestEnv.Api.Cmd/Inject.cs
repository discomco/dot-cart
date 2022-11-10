using DotCart.Drivers.EventStoreDB;
using DotCart.TestEnv.Engine;
using DotCart.TestEnv.Engine.Drivers;
using DotCart.TestEnv.Engine.Effects;

namespace DotCart.TestEnv.Api.Cmd;

public static class Inject
{
    public static IServiceCollection AddTestApp(this IServiceCollection services)
    {
        return services
                .AddConfiguredESDBClients()
                .AddESDBEventStoreDriver()
                .AddInitializeEngineWithThrottleUpStream()
                .AddESDBEngineEventFeeder()
                .AddEngineESDBProjectorDriver()
                .AddInitializeEffects()
                .AddStartEffects()
                .AddThrottleUpEffects()
            ;
    }
}