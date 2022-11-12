using DotCart.Context.Drivers;
using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using DotCart.Contract.Schemas;
using DotCart.Core;
using DotCart.Drivers.EventStoreDB.Interfaces;
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
    private readonly int _maxRetries = Polly.Config.MaxRetries;
    private readonly AsyncRetryPolicy _retryPolicy;

    private IReactor _reactor;
    private PersistentSubscription _subscription;

    public ESDBProjectorDriver(
        IESDBPersistentSubscriptionsClient client,
        SubscriptionFilterOptions filterOptions,
        AsyncRetryPolicy? retryPolicy = null)
    {
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
        if (_subscription != null) _subscription.Dispose();
    }

    public void SetReactor(IReactor reactor)
    {
        _reactor = reactor;
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
                Log.Error($"::EXCEPTION {e.Message})");
                throw;
            }
        });
    }


    public Task StopStreamingAsync(CancellationToken cancellationToken)
    {
        Log.Information("EventStore: Stopped Listening");
        return Task.CompletedTask;
    }


    private async Task CreateSubscription(CancellationToken cancellationToken)
    {
        // await _retryPolicy.ExecuteAsync(async () =>
        // {
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
            if (e.StatusCode != StatusCode.AlreadyExists) throw;
        }
        catch (Exception e)
        {
            Log.Error(e.InnerAndOuter());
        }
        // });
    }

    private void SubscriptionDropped(PersistentSubscription subscription, SubscriptionDroppedReason reason,
        Exception? exception)
    {
    }

    private async Task EventAppeared(
        PersistentSubscription subscription,
        ResolvedEvent resolvedEvent,
        int? someInt,
        CancellationToken cancellationToken)
    {
        var evt = resolvedEvent.Event.ToEvent();
        Log.Information($"\u001b[33m {subscription.SubscriptionId}\u001b[0m=> {evt.AggregateID.Id()} ~> {evt.Topic}");
        await _reactor.HandleAsync(evt, cancellationToken).ConfigureAwait(false);
        await subscription.Ack(resolvedEvent);
    }
}