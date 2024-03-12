using EventStore.Client;

namespace DotCart.Drivers.EventStoreDB;

/// <summary>
///     The client used to manage persistent subscriptions in the EventStoreDB.
/// </summary>
/// <footer>
///     <a href="https://www.google.com/search?q=EventStore.Client.EventStorePersistentSubscriptionsClient">
///         `EventStorePersistentSubscriptionsClient`
///         on google.com
///     </a>
/// </footer>
public interface IResilientPersistentSubscriptionsESDBClient
    : IESDBClientBase
{
    Task CreateAsync(
        string streamName,
        string groupName,
        PersistentSubscriptionSettings settings,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);

    Task CreateToAllAsync(
        string groupName,
        IEventFilter eventFilter,
        PersistentSubscriptionSettings settings,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);

    Task CreateToAllAsync(
        string groupName,
        PersistentSubscriptionSettings settings,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string streamName,
        string groupName,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);

    Task DeleteToAllAsync(
        string groupName,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);

    // Task<PersistentSubscription> SubscribeAsync(
    //     string streamName,
    //     string groupName,
    //     Func<PersistentSubscription, ResolvedEvent, int?, CancellationToken, Task> eventAppeared,
    //     Action<PersistentSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped = null,
    //     UserCredentials? userCredentials = null,
    //     int bufferSize = 10,
    //     bool autoAck = true,
    //     CancellationToken cancellationToken = default(CancellationToken));

    // Task<PersistentSubscription> SubscribeAsync(
    //     string streamName,
    //     string groupName,
    //     Func<PersistentSubscription, ResolvedEvent, int?, CancellationToken, Task> eventAppeared,
    //     Action<PersistentSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped = null,
    //     UserCredentials? userCredentials = null,
    //     int bufferSize = 10,
    //     CancellationToken cancellationToken = default);

    Task<PersistentSubscription> SubscribeToStreamAsync(
        string streamName,
        string groupName,
        Func<PersistentSubscription, ResolvedEvent, int?, CancellationToken, Task> eventAppeared,
        Action<PersistentSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped = null,
        UserCredentials? userCredentials = null,
        int bufferSize = 10,
        CancellationToken cancellationToken = default);

    Task<PersistentSubscription> SubscribeToAllAsync(
        string groupName,
        Func<PersistentSubscription, ResolvedEvent, int?, CancellationToken, Task> eventAppeared,
        Action<PersistentSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped = null,
        UserCredentials? userCredentials = null,
        int bufferSize = 10,
        CancellationToken cancellationToken = default);


    Task UpdateAsync(
        string streamName,
        string groupName,
        PersistentSubscriptionSettings settings,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);

    Task UpdateToAllAsync(
        string groupName,
        PersistentSubscriptionSettings settings,
        TimeSpan? deadline = null,
        UserCredentials? userCredentials = null,
        CancellationToken cancellationToken = default);
}