using DotCart.Drivers.Mediator;
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
            .AddExchange()
            .AddESDBEngineEventFeeder()
            .AddESDBInfra<Spoke>()
            .AddInitializeSpoke();
    }
}