using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Drivers.EventStoreDB.Interfaces;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;

namespace DotCart.Drivers.EventStoreDB;

public static partial class Inject
{
    public static IServiceCollection AddESDBStore(this IServiceCollection services)
    {
        return services
            .AddConfiguredESDBClients()
            .AddSingleton<IAggregateStore, ESDBStore>()
            .AddSingleton<IEventStore, ESDBStore>();
    }
}

public class ESDBStore : IEventStore
{
    private readonly IESDBEventSourcingClient _client;
    private readonly int _maxRetries = Polly.Config.MaxRetries;
    private readonly AsyncRetryPolicy _retryPolicy;
    private IActor _actor;

    public ESDBStore(
        IESDBEventSourcingClient client,
        AsyncRetryPolicy? retryPolicy = null
    )
    {
        _client = client;
        _retryPolicy = retryPolicy
                       ?? Policy
                           .Handle<Exception>()
                           .WaitAndRetryAsync(_maxRetries,
                               times => TimeSpan.FromMilliseconds(times * 100));
    }

    public void Dispose()
    {
    }


    public void SetActor(IActor actor)
    {
        _actor = actor;
    }

    public void Close()
    {
    }

    public async Task LoadAsync(IAggregate aggregate, CancellationToken cancellationToken = default)
    {
        var events = await ReadEventsAsync(aggregate.ID, cancellationToken);
        aggregate.Load(events);
    }

    public async Task<AppendResult> SaveAsync(IAggregate aggregate, CancellationToken cancellationToken = default)
    {
        var res = await AppendEventsAsync(aggregate.ID, aggregate.UncommittedEvents, cancellationToken);
        aggregate.ClearUncommittedEvents(res.NextExpectedVersion);
        return res;
    }


    public async Task<IEnumerable<IEvtB>> ReadEventsAsync(IID ID, CancellationToken cancellationToken = default)
    {
        var ret = new List<Event>();
        var readResult = _client.ReadStreamAsync(Direction.Forwards,
            ID.Id(),
            StreamPosition.Start,
            long.MaxValue,
            false,
            null,
            null,
            cancellationToken);
        if (readResult == null)
            throw new ESDBException("ESDBClient returned no readResult.");
        return await GetStoreEventsAsync(readResult, cancellationToken);
    }


    public Task<AppendResult> AppendEventsAsync(IID ID, IEnumerable<IEvtB> events,
        CancellationToken cancellationToken = default)
    {
        return _retryPolicy.ExecuteAsync(async () =>
        {
            var storeEvents = new List<EventData>();
            storeEvents = events.Aggregate(storeEvents, (list, evt) =>
            {
                list.Add(evt.ToEventData());
                return list;
            });
            var writeResult = await _client.AppendToStreamAsync(ID.Id(),
                StreamState.Any,
                storeEvents,
                cancellationToken: cancellationToken);
            return AppendResult.New(writeResult.NextExpectedStreamRevision.ToUInt64());
        });
    }


    private static async Task<IEnumerable<IEvtB>> GetStoreEventsAsync(EventStoreClient.ReadStreamResult readResult,
        CancellationToken cancellationToken = default)
    {
        var res = new List<IEvtB>();
        var state = await readResult.ReadState.ConfigureAwait(false);
        if (state == ReadState.StreamNotFound)
            return res;
        await foreach (var evt in readResult)
            res.Add(evt.Event.ToEvent());
        return res;
    }

    public ValueTask DisposeAsync()
    {
        return _client.DisposeAsync();
    }
}