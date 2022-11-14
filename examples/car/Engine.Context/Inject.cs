using DotCart.Context.Abstractions;
using DotCart.Context.Effects;
using DotCart.Drivers.EventStoreDB;
using Engine.Context.Common.Drivers;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context;

public static class Inject
{
    public static IServiceCollection AddESDBInfra<TSpoke>(this IServiceCollection services)
        where TSpoke : ISpokeT<TSpoke>
    {
        return services
            .AddProjector()
            .AddConfiguredESDBClients()
            .AddESDBEventStoreDriver()
            .AddEngineESDBProjectorDriver<TSpoke>();
    }
}