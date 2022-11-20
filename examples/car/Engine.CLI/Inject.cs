using DotCart.Drivers.EventStoreDB;
using DotCart.Drivers.Mediator;
using Engine.Context.ChangeRpm;
using Engine.Context.Common.Drivers;
using Engine.Context.Common.Effects;
using Engine.Context.Initialize;
using Engine.Context.Start;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.CLI;

public static class Inject
{
    public static IServiceCollection BuildTestApp(this IServiceCollection services)
    {
        return services
//            .AddCartwheel()
            .AddSingletonExchange()
            .AddESDBInfra()
            .AddSingletonESDBProjectorDriver<IEngineSubscriptionInfo>()
            .AddESDBEngineEventFeeder()
            .AddInitializeSpoke()
            .AddStartSpoke()
            .AddChangeRpmSpoke();
    }
}