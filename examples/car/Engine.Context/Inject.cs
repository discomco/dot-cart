using DotCart.Context.Abstractions;
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
            .AddConfiguredESDBClients()
            .AddESDBEventStoreDriver()
            .AddEngineESDBProjectorDriver<TSpoke>();
    }
}