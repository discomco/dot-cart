using DotCart.Behavior;
using DotCart.Drivers.EventStoreDB.Interfaces;
using DotCart.Effects;
using DotCart.Effects.Drivers;
using DotCart.Schema;
using EventStore.Client;
using Grpc.Core;
using Polly;
using Polly.Retry;
using Serilog;


namespace DotCart.Drivers.EventStoreDB;

public class ESDBProjectorDriver<TInfo> : IProjectorDriver<TInfo> where TInfo : ISubscriptionInfo
{
    private readonly IESDBPersistentSubscriptionsClient _client;

    private readonly SubscriptionFilterOptions _filterOptions;
    private readonly AsyncRetryPolicy _retryPolicy;

    private IReactor _reactor;
    private PersistentSubscription _subscription;
    private readonly ILogger _logger;
    private readonly int _maxRetries = Polly.Config.MaxRetries;

    public ESDBProjectorDriver(
        ILogger logger,
        IESDBPersistentSubscriptionsClient client,
        SubscriptionFilterOptions filterOptions,
        AsyncRetryPolicy? retryPolicy = null)
    {
        _logger = logger;
        _client = client;
        _filterOptions = filterOptions;
        _retryPolicy = retryPolicy
                       ?? retryPolicy
                       ?? Policy
                           .Handle<Exception>()
                           .WaitAndRetryAsync(_maxRetries,
                               times => TimeSpan.FromMilliseconds(times * 100));
    }

    public void Dispose()
    {
        _subscription.Dispose();
    }

    public void SetReactor(IReactor reactor)
    {
        _reactor = reactor;
    }


    private async Task CreateSubscription(CancellationToken cancellationToken)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            try
            {
                await _client.CreateToAllAsync(
                    GroupName.Get<TInfo>(),
                    EventTypeFilter.Prefix($"{IDPrefix.Get<TInfo>()}"),
                    new PersistentSubscriptionSettings(),
                    null,
                    null, cancellationToken);
            }
            catch (RpcException e)
            {
                if (e.StatusCode != StatusCode.AlreadyExists)
                {
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.InnerAndOuter());
            }
        });
    }

    public async Task StartStreamingAsync(CancellationToken cancellationToken)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            try
            {
                await CreateSubscription(cancellationToken);

                _subscription = await _client.SubscribeToAllAsync(
                    GroupName.Get<TInfo>(),
                    EventAppeared,
                    SubscriptionDropped,
                    null,
                    10,
                    cancellationToken);
            }
            catch (Exception e)
            {
                _logger.Error($"::EXCEPTION {e.Message})");
                throw;
            }
        });
    }

    private void SubscriptionDropped(PersistentSubscription subscription, SubscriptionDroppedReason reason,
        Exception? exception)
    {
    }

    private Task EventAppeared(
        PersistentSubscription subscription,
        ResolvedEvent resolvedEvent,
        int? someInt,
        CancellationToken cancellationToken)
    {
        var evt = resolvedEvent.Event.ToEvent();
        _logger.Information($"::RESOLVED => {evt.MsgId}");
        return _reactor.HandleAsync(evt, cancellationToken);
    }


    public Task StopStreamingAsync(CancellationToken cancellationToken)
    {
        _logger?.Information("EventStore: Stopped Listening");
        return Task.CompletedTask;
    }
}