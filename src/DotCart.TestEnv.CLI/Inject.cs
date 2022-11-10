using DotCart.Drivers.EventStoreDB;
using DotCart.Drivers.Serilog;
using DotCart.TestEnv.Engine;
using DotCart.TestEnv.Engine.Drivers;
using DotCart.TestEnv.Engine.Effects;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestEnv.CLI;

public static class Inject
{
    public static IServiceCollection BuildTestApp(this IServiceCollection services)
    {
        return services
                .AddConfiguredESDBClients()
                .AddESDBEventStoreDriver()
                .AddInitializeEngineWithThrottleUpStream()
                .AddEngineESDBProjectorDriver()
                .AddESDBEngineEventFeeder()
                .AddInitializeEffects()
                .AddStartEffects()
                .AddThrottleUpEffects()
            ;
    }
}