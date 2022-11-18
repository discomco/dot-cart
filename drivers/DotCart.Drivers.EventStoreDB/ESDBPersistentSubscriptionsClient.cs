using DotCart.Drivers.EventStoreDB.Interfaces;
using EventStore.Client;
using Polly;
using Polly.Retry;

namespace DotCart.Drivers.EventStoreDB;

internal class ESDBPersistentSubscriptionsClient : IESDBPersistentSubscriptionsClient
{
    private readonly EventStorePersistentSubscriptionsClient _clt;
    private readonly int _maxRetries = Polly.Config.MaxRetries;
    private readonly AsyncRetryPolicy _retryPolicy;

    public ESDBPersistentSubscriptionsClient(EventStorePersistentSubscriptionsClient clt,
        AsyncRetryPolicy? retryPolicy)
    {
        _clt = clt;
        _retryPolicy = retryPolicy
                       ?? Policy
                           .Handle<Exception>()
                           .WaitAndRetryAsync(_maxRetries,
                               times => TimeSpan.FromMilliseconds(times * 100));
    }

    public void Dispose()
    {
        _clt
            .Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _clt
            .DisposeAsync();
    }

    public string ConnectionName => _clt.ConnectionName;

    public Task CreateAsync(string streamName, string groupName, PersistentSubscriptionSettings settings,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null, CancellationToken cancellationToken = default)
    {
//        return _retryPolicy.ExecuteAsync(() => 

        return _clt.CreateAsync(streamName,
            groupName,
            settings,
            deadline,
            userCredentials,
            cancellationToken);

//            );
    }

    public Task CreateToAllAsync(string groupName, IEventFilter eventFilter, PersistentSubscriptionSettings settings,
        TimeSpan? deadline = null, UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default)
    {
//        return _retryPolicy.ExecuteAsync(() =>

        return _clt.CreateToAllAsync(groupName,
            eventFilter,
            settings,
            deadline,
            userCredentials,
            cancellationToken);

        //);
    }

    public Task CreateToAllAsync(string groupName, PersistentSubscriptionSettings settings, TimeSpan? deadline = null,
        UserCredentials? userCredentials = null, CancellationToken cancellationToken = default)
    {
//        return _retryPolicy.ExecuteAsync(() => 

        return _clt.CreateToAllAsync(groupName,
            settings,
            deadline,
            userCredentials,
            cancellationToken);

        //);
    }

    public Task DeleteAsync(string streamName, string groupName,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default)
    {
//        return _retryPolicy.ExecuteAsync(() => 
        return _clt.DeleteAsync(streamName,
            groupName,
            deadline,
            userCredentials,
            cancellationToken);
//            );
    }

    public Task DeleteToAllAsync(string groupName, TimeSpan? deadline = null, UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default)
    {
//        return _retryPolicy.ExecuteAsync(() =>
        return _clt.DeleteToAllAsync(groupName,
            deadline,
            userCredentials,
            cancellationToken);
        //              );
    }

    // public Task<PersistentSubscription> SubscribeAsync(string streamName, string groupName, Func<PersistentSubscription, ResolvedEvent, int?, CancellationToken, Task> eventAppeared, Action<PersistentSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped = null,
    //     UserCredentials? userCredentials = null, int bufferSize = 10, bool autoAck = true,
    //     CancellationToken cancellationToken = default)
    // {
    //     return _clt.Su
    // }

    public Task<PersistentSubscription> SubscribeToStreamAsync(
        string streamName,
        string groupName,
        Func<PersistentSubscription, ResolvedEvent, int?, CancellationToken, Task> eventAppeared,
        Action<PersistentSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped = null,
        UserCredentials? userCredentials = null, int bufferSize = 10, CancellationToken cancellationToken = default)
    {
//        return _retryPolicy.ExecuteAsync(() => 

        return _clt.SubscribeToStreamAsync(
            streamName,
            groupName,
            eventAppeared,
            subscriptionDropped,
            userCredentials,
            bufferSize,
            cancellationToken);
//                );
    }

    public Task<PersistentSubscription> SubscribeToAllAsync(
        string groupName,
        Func<PersistentSubscription, ResolvedEvent, int?, CancellationToken, Task> eventAppeared,
        Action<PersistentSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped = null,
        UserCredentials? userCredentials = null,
        int bufferSize = 10,
        CancellationToken cancellationToken = default)
    {
//        return _retryPolicy.ExecuteAsync(() => 

        return _clt.SubscribeToAllAsync(
            groupName,
            eventAppeared,
            subscriptionDropped,
            userCredentials,
            bufferSize,
            cancellationToken);

//            );
    }

    public Task UpdateAsync(
        string streamName,
        string groupName,
        PersistentSubscriptionSettings settings,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default)
    {
//        return _retryPolicy.ExecuteAsync(() => 

        return _clt.UpdateAsync(streamName,
            groupName,
            settings,
            deadline,
            userCredentials,
            cancellationToken);

        //);
    }

    public Task UpdateToAllAsync(
        string groupName,
        PersistentSubscriptionSettings settings,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default)
    {
        //return _retryPolicy.ExecuteAsync(() => 

        return _clt.UpdateToAllAsync(
            groupName,
            settings,
            deadline,
            userCredentials,
            cancellationToken);


        //);
    }

    public Task<PersistentSubscription> SubscribeAsync(
        string streamName,
        string groupName,
        Func<PersistentSubscription, ResolvedEvent, int?, CancellationToken, Task> eventAppeared,
        Action<PersistentSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped = null,
        UserCredentials? userCredentials = null,
        int bufferSize = 10,
        CancellationToken cancellationToken = default)
    {
//        return _retryPolicy.ExecuteAsync(() => 

        return _clt.SubscribeToStreamAsync(
            streamName,
            groupName,
            eventAppeared,
            subscriptionDropped,
            userCredentials,
            bufferSize,
            cancellationToken);

//            );
    }
}