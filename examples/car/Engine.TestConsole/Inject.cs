using DotCart.Drivers.EventStoreDB;
using Engine.Context;
using Engine.Context.ChangeRpm;
using Engine.Context.Common;
using Engine.Context.Common.Drivers;
using Engine.Context.Common.Effects;
using Engine.Context.Initialize;
using Engine.Context.Start;
using Microsoft.Extensions.DependencyInjection;
using Spoke = Engine.Context.Initialize.Spoke;

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