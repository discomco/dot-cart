using DotCart.Polly;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace DotCart.Drivers.EventStoreDB;

public static partial class Inject
{
    public static IServiceCollection AddResilientESDBClients(
        this IServiceCollection services,
        Action<EventStoreClientSettings>? overrideSettings = null)
    {
        var esdbSettings = services.BuildServiceProvider()
            .GetRequiredService<IOptions<ESDBSettings>>()
            .Value;
        return services
            .AddPolly()
            .AddESDBClients(esdbSettings.Uri, overrideSettings)
            .AddSingleton<IResilientPersistentSubscriptionsESDBClient, ResilientPersistentSubscriptionsESDBClient>()
            .AddSingleton<IResilientESDBClient, ResilientESDBClient>();
    }
}

internal class ResilientESDBClient
    : IResilientESDBClient
{
    private readonly EventStoreClient _client;
    private readonly int _maxRetries = Polly.Config.MaxRetries;
    private readonly AsyncRetryPolicy? _retryPolicy;


    public ResilientESDBClient(EventStoreClient client, AsyncRetryPolicy? retryPolicy = null)
    {
        _client = client;

        _retryPolicy = retryPolicy
                       ?? Policy
                           .Handle<Exception>()
                           .WaitAndRetryAsync(_maxRetries,
                               times => TimeSpan.FromMilliseconds(times * 100));
    }

    public Task<IWriteResult> AppendToStreamAsync(
        string streamName,
        StreamRevision expectedRevision,
        IEnumerable<EventData> eventData,
        Action<EventStoreClientOperationOptions>? configureOperationOptions = null,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default)
    {
        //      return _retryPolicy.ExecuteAsync(() =>
        return _client.AppendToStreamAsync(
            streamName,
            expectedRevision,
            eventData,
            configureOperationOptions,
            deadline,
            userCredentials, cancellationToken);
        //            );
    }

    public async Task<IWriteResult> AppendToStreamAsync(
        string streamName,
        StreamState expectedState,
        IEnumerable<EventData> eventData,
        Action<EventStoreClientOperationOptions>? configureOperationOptions = null,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            //        return _retryPolicy.ExecuteAsync(() => 
            var res = await _client.AppendToStreamAsync(streamName,
                expectedState,
                eventData,
                configureOperationOptions,
                deadline,
                userCredentials, cancellationToken);
            //            );
            return res;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<StreamMetadataResult> GetStreamMetadataAsync(string streamName,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default)
    {
        //        return _retryPolicy.ExecuteAsync(() =>
        return _client.GetStreamMetadataAsync(
            streamName,
            deadline,
            userCredentials,
            cancellationToken);
        //          );
    }

    public Task<IWriteResult> SetStreamMetadataAsync(
        string streamName,
        StreamState expectedState,
        StreamMetadata metadata,
        Action<EventStoreClientOperationOptions>? configureOperationOptions = null,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default)
    {
        //        return _retryPolicy.ExecuteAsync(() => 
        return _client.SetStreamMetadataAsync(
            streamName,
            expectedState,
            metadata,
            configureOperationOptions,
            deadline,
            userCredentials,
            cancellationToken);
        //                );
    }

    public Task<IWriteResult> SetStreamMetadataAsync(
        string streamName,
        StreamRevision expectedRevision,
        StreamMetadata metadata,
        Action<EventStoreClientOperationOptions>? configureOperationOptions = null,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default)
    {
        //        return _retryPolicy.ExecuteAsync(() => 

        return _client.SetStreamMetadataAsync(
            streamName,
            expectedRevision,
            metadata,
            configureOperationOptions,
            deadline,
            userCredentials, cancellationToken);
        //          );
    }

    public IAsyncEnumerable<ResolvedEvent> ReadAllAsync(
        Direction direction,
        Position position,
        long maxCount = 9223372036854775807,
        bool resolveLinkTos = false,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default)
    {
        return _client.ReadAllAsync(
            direction,
            position,
            maxCount,
            resolveLinkTos,
            deadline,
            userCredentials, cancellationToken);
    }

    public EventStoreClient.ReadStreamResult ReadStreamAsync(
        Direction direction,
        string streamName,
        StreamPosition revision,
        long maxCount = 9223372036854775807,
        bool resolveLinkTos = false,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null, CancellationToken cancellationToken = default)
    {
        return _client.ReadStreamAsync(direction,
            streamName, revision, maxCount,
            resolveLinkTos, deadline, userCredentials, cancellationToken);
    }

    public Task<StreamSubscription> SubscribeToAllAsync(
        FromAll startFrom,
        Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppeared, bool resolveLinkTos = false,
        Action<StreamSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped = null,
        SubscriptionFilterOptions? filterOptions = null,
        UserCredentials? userCredentials = null, CancellationToken cancellationToken = default)
    {
        //        return _retryPolicy.ExecuteAsync(() => 

        return _client.SubscribeToAllAsync(
            startFrom,
            eventAppeared,
            resolveLinkTos,
            subscriptionDropped,
            filterOptions,
            userCredentials, cancellationToken);
        //                );
    }

    // public Task<StreamSubscription> SubscribeToAllAsync(Position start,
    //     Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppeared, bool resolveLinkTos = false,
    //     Action<StreamSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped = null,
    //     SubscriptionFilterOptions? filterOptions = null,
    //     Action<EventStoreClientOperationOptions>? configureOperationOptions = null,
    //     UserCredentials? userCredentials = null,
    //     CancellationToken cancellationToken = default)
    // {
    //     return _retryPolicy.ExecuteAsync(() => _client.SubscribeToAllAsync(start,
    //         eventAppeared,
    //         resolveLinkTos,
    //         subscriptionDropped,
    //         filterOptions,
    //         configureOperationOptions, userCredentials, cancellationToken));
    // }

    // public Task<StreamSubscription> SubscribeToStreamAsync(
    //     string streamName,
    //     Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppeared, bool resolveLinkTos = false,
    //     Action<StreamSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped = null,
    //     Action<EventStoreClientOperationOptions>? configureOperationOptions = null,
    //     UserCredentials? userCredentials = null, CancellationToken cancellationToken = default)
    // {
    //     return _retryPolicy.ExecuteAsync(() => _client.SubscribeToStreamAsync(
    //         streamName,
    //         eventAppeared,
    //         resolveLinkTos,
    //         subscriptionDropped,
    //         configureOperationOptions, userCredentials, cancellationToken));
    // }

    public Task<StreamSubscription> SubscribeToStreamAsync(
        string streamName,
        FromStream start,
        Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppeared,
        bool resolveLinkTos = false,
        Action<StreamSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default)
    {
        //return _retryPolicy.ExecuteAsync(() => 
        return _client.SubscribeToStreamAsync(
            streamName,
            start,
            eventAppeared,
            resolveLinkTos,
            subscriptionDropped,
            userCredentials,
            cancellationToken);
        //              );
    }

    public Task<DeleteResult> TombstoneAsync(string streamName,
        StreamRevision expectedRevision,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null, CancellationToken cancellationToken = default)
    {
        //        return _retryPolicy.ExecuteAsync(() => 

        return _client.TombstoneAsync(
            streamName,
            expectedRevision,
            deadline,
            userCredentials,
            cancellationToken);

        //          );
    }

    public Task<DeleteResult> TombstoneAsync(
        string streamName,
        StreamState expectedState,
        TimeSpan? deadline,
        UserCredentials? userCredentials = null, CancellationToken cancellationToken = default)
    {
        //        return _retryPolicy.ExecuteAsync(() => 

        return _client.TombstoneAsync(
            streamName,
            expectedState,
            deadline,
            userCredentials,
            cancellationToken);

        //               );
    }

    public void Dispose()
    {
        _client
            .Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _client
            .DisposeAsync();
    }

    public string ConnectionName => _client.ConnectionName;
}