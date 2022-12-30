using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Drivers.EventStoreDB.Interfaces;
using DotCart.Drivers.Polly;
using EventStore.Client;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.EventStoreDB;

public static partial class Inject
{
    public static IServiceCollection AddConfiguredESDBClients(this IServiceCollection services)
    {
        return services
            .AddPolly()
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


    public static IServiceCollection AddSingletonESDBProjectorDriver<TInfo>(this IServiceCollection services)
        where TInfo : ISubscriptionInfoB
    {
        return services
            .AddSingleton(_ =>
                new SubscriptionFilterOptions(
                    StreamFilter.Prefix($"{IDPrefixAtt.Get<TInfo>()}{IDFuncs.PrefixSeparator}")))
            .AddSingleton<IProjectorDriverT<TInfo>, ESDBProjectorDriver<TInfo>>();
    }

    public static IServiceCollection AddTransientESDBProjectorDriver<TInfo>(this IServiceCollection services)
        where TInfo : ISubscriptionInfoB
    {
        return services
            .AddConfiguredESDBClients()
            .AddSingleton(_ =>
                new SubscriptionFilterOptions(
                    StreamFilter.Prefix($"{IDPrefixAtt.Get<TInfo>()}{IDFuncs.PrefixSeparator}")))
            .AddTransient<IProjectorDriverT<TInfo>, ESDBProjectorDriver<TInfo>>();
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