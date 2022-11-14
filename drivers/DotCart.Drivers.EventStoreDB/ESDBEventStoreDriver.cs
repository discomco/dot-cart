using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Contract.Schemas;
using DotCart.Drivers.EventStoreDB.Interfaces;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;

namespace DotCart.Drivers.EventStoreDB;

public static partial class Inject
{
    public static IServiceCollection AddESDBEventStoreDriver(this IServiceCollection services)
    {
        return services
            .AddSingleton<IAggregateStoreDriver, ESDBEventStoreDriver>()
            .AddSingleton<IEventStoreDriver, ESDBEventStoreDriver>();
    }
}

public class ESDBEventStoreDriver : IEventStoreDriver
{
    private readonly IESDBEventSourcingClient _client;
    private readonly int _maxRetries = Polly.Config.MaxRetries;
    private readonly AsyncRetryPolicy _retryPolicy;
    private IActor _actor;

    public ESDBEventStoreDriver(
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


    public async Task<IEnumerable<IEvt>> ReadEventsAsync(IID ID, CancellationToken cancellationToken = default)
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
            throw new ESDBEventStoreDriverException("ESDBClient returned no readResult.");
        return await GetStoreEventsAsync(readResult, cancellationToken);
    }


    public Task<AppendResult> AppendEventsAsync(IID ID, IEnumerable<IEvt> events,
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


    private static async Task<IEnumerable<IEvt>> GetStoreEventsAsync(EventStoreClient.ReadStreamResult readResult,
        CancellationToken cancellationToken = default)
    {
        var res = new List<IEvt>();
        var state = await readResult.ReadState.ConfigureAwait(false);
        if (state == ReadState.StreamNotFound)
            return res;
        await foreach (var evt in readResult) res.Add(ToEvent(evt));

        return res;
    }

    private static Event ToEvent(ResolvedEvent evt)
    {
        return new Event(
            evt.Event.EventStreamId.IDFromIdString(),
            evt.Event.EventType,
            evt.Event.EventNumber.ToInt64(),
            evt.Event.Data.ToArray(),
            evt.Event.Metadata.ToArray(),
            evt.Event.Created)
        {
            EventId = evt.Event.EventId.ToString()
        };
    }

    public ValueTask DisposeAsync()
    {
        return _client.DisposeAsync();
    }
}