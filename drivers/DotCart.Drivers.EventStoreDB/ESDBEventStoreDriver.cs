using DotCart.Behavior;
using DotCart.Drivers.EventStoreDB.Interfaces;
using DotCart.Effects;
using DotCart.Effects.Drivers;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.EventStoreDB;


public static partial class Inject
{
    public static IServiceCollection AddESDBEventStoreDriver(this IServiceCollection services)
    {
        return services
            .AddConfiguredESDBClients()
            .AddSingleton<IAggregateStoreDriver, ESDBEventStoreDriver>()
            .AddSingleton<IEventStoreDriver, ESDBEventStoreDriver>();

    }
}







public class ESDBEventStoreDriver : IEventStoreDriver
{
    private readonly IESDBEventSourcingClient _client;
    private IReactor _reactor;

    public ESDBEventStoreDriver(IESDBEventSourcingClient client)
    {
        _client = client;
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public void SetReactor(IReactor reactor)
    {
        _reactor = reactor;
    }

    public void Close()
    {

    }

    public Task LoadAsync(IAggregate aggregate)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(IAggregate aggregate)
    {
        throw new NotImplementedException();
    }
}