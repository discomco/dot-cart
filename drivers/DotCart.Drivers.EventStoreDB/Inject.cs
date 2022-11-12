using DotCart.Drivers.EventStoreDB.Interfaces;
using EventStore.Client;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.EventStoreDB;

public static partial class Inject
{
    public static IServiceCollection AddConfiguredESDBClients(this IServiceCollection services)
    {
        return services?
            .AddEventStore(s =>
            {
                s.ConnectivitySettings.Insecure = Config.Insecure;
                s.ConnectivitySettings.Address = new Uri(Config.Uri);
                //  s.DefaultCredentials = new UserCredentials(Config.UserName, Config.Password);
                if (Config.UseTls) s.ChannelCredentials = new SslCredentials();
            })
            .AddSingleton<IESDBPersistentSubscriptionsClient, ESDBPersistentSubscriptionsClient>()
            .AddSingleton<IESDBEventSourcingClient, ESDBEventSourcingClient>();
    }


    private static IServiceCollection AddEventStore(this IServiceCollection services,
        Action<EventStoreClientSettings>? clientSettings)
    {
        return services
            .AddEventStoreClient(clientSettings)
            .AddEventStoreOperationsClient(clientSettings)
            .AddEventStorePersistentSubscriptionsClient(clientSettings)
            .AddEventStoreProjectionManagementClient(clientSettings)
            .AddEventStoreUserManagementClient(clientSettings);
    }
}