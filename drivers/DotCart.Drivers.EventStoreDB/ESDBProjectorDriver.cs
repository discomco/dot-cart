using DotCart.Drivers.EventStoreDB.Interfaces;
using DotCart.Effects;
using DotCart.Effects.Drivers;
using EventStore.Client;
using Serilog;


namespace DotCart.Drivers.EventStoreDB;

public class ESDBProjectorDriver<TInfo> : IProjectorDriver<TInfo> where TInfo: ISubscriptionInfo
{
    private readonly IESDBPersistentSubscriptionsClient _client;
    
    private readonly SubscriptionFilterOptions _filterOptions;

    private IReactor _reactor;
    private Task<PersistentSubscription> _subscription;
    private ILogger _logger;

    public ESDBProjectorDriver(
        ILogger logger,
        IESDBPersistentSubscriptionsClient client,
        SubscriptionFilterOptions filterOptions)
    {
        _logger = logger;
        _client = client;
        _filterOptions = filterOptions;
    }

    public void Dispose()
    {
        _subscription.Dispose();
    }

    public void SetReactor(IReactor reactor)
    {
        _reactor = reactor;
    }

    public Task StartStreamingAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            try
            {
                var request = string.Empty;
                _subscription = _client.SubscribeToAllAsync(
                    GroupName.Get<TInfo>(),
                    EventAppeared,
                    SubscriptionDropped,
                    null,
                    10,
                    cancellationToken);
            }
            catch (Exception e)
            {
                _logger?.Fatal($"::EXCEPTION {e.Message})");
                throw;
            }            
        }, cancellationToken);
    }

    private void SubscriptionDropped(PersistentSubscription subscription, SubscriptionDroppedReason reason, Exception? exception)
    {
        
    }

    private Task EventAppeared(PersistentSubscription subscription, 
        ResolvedEvent resolvedEvent, 
        int? someInt, 
        CancellationToken cancellationToken)
    {
        var evt = resolvedEvent.Event.ToEvent();
        return _reactor.HandleAsync(evt);
    }


    public Task StopStreamingAsync(CancellationToken cancellationToken)
    {
        _logger?.Debug("EventStore: Stopped Listening");
        return Task.CompletedTask;
    }


}