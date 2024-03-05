using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Drivers.EventStoreDB;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Defaults.EventStore;

public static class Drivers
{
    public static IServiceCollection AddProjectorInfra<TProjectionInfo, TDoc, TListDoc>(
        this IServiceCollection services)
        where TDoc : IState
        where TProjectionInfo : IProjectorInfoB
        where TListDoc : IState
    {
        return services
            .AddSingletonESDBProjector<TProjectionInfo>();
    }
}