using DotCart.Drivers.EventStoreDB;
using DotCart.Drivers.Mediator;
using Engine.Context.ChangeRpm;
using Engine.Context.Common.Drivers;
using Engine.Context.Initialize;
using Engine.Context.Start;

namespace Engine.Api.Cmd;

public static class Inject
{
    public static IServiceCollection BuildTestApp(this IServiceCollection services)
    {
        return services
//            .AddCartwheel()
            .AddSingletonExchange()
            .AddESDBInfra()
            .AddSingletonESDBProjector<IEngineSubscriptionInfo>()
            .AddInitializeSpoke();
        ;
        ;
        // .AddStartSpoke()
        // .AddChangeRpmSpoke();
    }
}