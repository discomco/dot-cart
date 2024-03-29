using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Actors;
using DotCart.Core;
using DotCart.Schema;
using EventStore.Client;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;
using Serilog;

namespace DotCart.Drivers.EventStoreDB;

public static partial class Inject
{
    public static IServiceCollection AddSingletonESDBProjector<TInfo>(
        this IServiceCollection services,
        Action<EventStoreClientSettings>? overrideSettings = null)
        where TInfo : IProjectorInfoB
    {
        return services
            .AddResilientESDBClients(overrideSettings)
            .AddSingletonESDBProjectorDriver<TInfo>()
            .AddSingleton<IProjector, Projector<TInfo>>();
    }

    public static IServiceCollection AddTransientESDBProjector<TInfo>(
        this IServiceCollection services,
        string connectionString, Action<EventStoreClientSettings>? overrideSettings = null)
        where TInfo : IProjectorInfoB
    {
        return services
            .AddResilientESDBClients(overrideSettings)
            .AddTransientESDBProjectorDriver<TInfo>(overrideSettings)
            .AddTransient<IProjector, Projector<TInfo>>();
    }

    public static IServiceCollection AddSingletonESDBProjectorDriver<TInfo>(this IServiceCollection services,
        Action<EventStoreClientSettings>? overrideSettings = null)
        where TInfo : IProjectorInfoB
    {
        return services
            .AddSingleton(_ =>
                new SubscriptionFilterOptions(
                    StreamFilter.Prefix($"{IDPrefixAtt.Get<TInfo>()}{IDFuncs.PrefixSeparator}")))
            .AddSingleton<IProjectorDriverT<TInfo>, ESDBProjectorDriver<TInfo>>();
    }

    public static IServiceCollection AddTransientESDBProjectorDriver<TInfo>(this IServiceCollection services,
        Action<EventStoreClientSettings>? overrideSettings = null)
        where TInfo : IProjectorInfoB
    {
        return services
            .AddResilientESDBClients(overrideSettings)
            .AddSingleton(_ =>
                new SubscriptionFilterOptions(
                    StreamFilter.Prefix($"{IDPrefixAtt.Get<TInfo>()}{IDFuncs.PrefixSeparator}")))
            .AddTransient<IProjectorDriverT<TInfo>, ESDBProjectorDriver<TInfo>>();
    }
}

public class ESDBProjectorDriver<TInfo>
    : DriverB, IProjectorDriverT<TInfo> where TInfo : IProjectorInfoB
{
    private readonly IResilientPersistentSubscriptionsESDBClient _client;
    private readonly SubscriptionFilterOptions _filterOptions;
    private readonly int _maxRetries = Polly.Config.MaxRetries;

    private readonly AsyncRetryPolicy _retryPolicy;
    //private IActor _actor;

    private CancellationTokenSource _cts;

    private PersistentSubscription _subscription;
    private bool _subscriptionExists;


    public ESDBProjectorDriver(
        IResilientPersistentSubscriptionsESDBClient client,
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


    public Task<IEvtB> CreateEventAsync(object source, CancellationToken cancellationToken = default)
    {
        return (Task<IEvtB>)Task.CompletedTask;
    }


    public override void Dispose()
    {
        if (_subscription != null)
            _subscription.Dispose();
    }

    public async Task StartStreamingAsync(CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        try
        {
            await CreateSubscription(_cts.Token);
            _subscription = await _client.SubscribeToAllAsync(
                GroupNameAtt.Get<TInfo>(),
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
    }


    public Task StopStreamingAsync(CancellationToken cancellationToken)
    {
        Log.Information("EventStore: Stopped Listening");
        _cts.Cancel();
        return Task.CompletedTask;
    }

    // public void SetActor(IActor actor)
    // {
    //     _actor = actor;
    // }


    private async Task CreateSubscription(CancellationToken cancellationToken)
    {
        // await _retryPolicy.ExecuteAsync(async () =>
        // {
        if (_subscriptionExists) return;
        try
        {
            await _client.CreateToAllAsync(
                GroupNameAtt.Get<TInfo>(),
                EventTypeFilter.Prefix($"{IDPrefixAtt.Get<TInfo>()}"),
                new PersistentSubscriptionSettings(),
                null,
                null, cancellationToken);
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.AlreadyExists)
                _subscriptionExists = true;
            else
                throw;
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
        _subscriptionExists = false;
        StartStreamingAsync(_cts.Token);
    }

    private async Task EventAppeared(
        PersistentSubscription subscription,
        ResolvedEvent resolvedEvent,
        int? someInt,
        CancellationToken cancellationToken)
    {
        var evt = resolvedEvent.Event.ToEvent();
        Log.Information($"\u001b[33m {subscription.SubscriptionId}\u001b[0m=> {evt.AggregateID.Id()} ~> {evt.Topic}");
        await Cast(evt, cancellationToken).ConfigureAwait(false);
        await subscription.Ack(resolvedEvent);
    }
}