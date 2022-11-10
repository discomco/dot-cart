using DotCart.Behavior;
using DotCart.Drivers.EventStoreDB.Interfaces;
using DotCart.Effects;
using DotCart.Effects.Drivers;
using DotCart.Schema;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;


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
    private IReactor _reactor;

    public ESDBEventStoreDriver(IESDBEventSourcingClient client)
    {
        _client = client;
    }

    public void Dispose()
    {
        
    }

    public void SetReactor(IReactor reactor)
    {
        _reactor = reactor;
    }

    public void Close()
    {
    }

    public async Task LoadAsync(IAggregate aggregate, CancellationToken cancellationToken = default)
    {
        var events = await ReadEventsAsync(aggregate.ID, cancellationToken);
        aggregate.Load(events);
    }

    public Task SaveAsync(IAggregate aggregate, CancellationToken cancellationToken = default)
    {
        var res = AppendEventsAsync(aggregate.ID, aggregate.UncommittedEvents);
        aggregate.ClearUncommittedEvents();
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
        if (readResult==null) 
            throw new ESDBEventStoreDriverException("ESDBClient returned no readResult.");
       var state = await readResult.ReadState.ConfigureAwait(false);
       if (state == ReadState.StreamNotFound) 
           return ret;
        return await GetStoreEventsAsync(readResult);
    }


    private static async Task<IEnumerable<IEvt>> GetStoreEventsAsync(EventStoreClient.ReadStreamResult readResult,
        CancellationToken cancellationToken = default)
    {
        var res = new List<IEvt>();
        await foreach (var evt in readResult)
        {
            var eOut = new Event(
                evt.Event.EventStreamId.IDFromIdString(),
                evt.Event.EventType,
                evt.Event.EventNumber.ToInt64(),
                evt.Event.Data.ToArray(),
                evt.Event.Metadata.ToArray(),
                evt.Event.Created)
            {
                EventId = evt.Event.EventId.ToString()
            };
            res.Add(eOut);
        }

        return res;
    }


    public async Task<AppendResult> AppendEventsAsync(IID ID, IEnumerable<IEvt> events, CancellationToken cancellationToken = default)
    {
        var storeEvents = new List<EventData>();
        storeEvents = events.Aggregate(storeEvents, (list, evt) =>
        {
            list.Add(evt.ToEventData());
            return list;
        });
        var writeResult = await _client.AppendToStreamAsync(ID.Id(), StreamState.Any, storeEvents);
        return AppendResult.New(writeResult.NextExpectedStreamRevision.ToUInt64());
    }


    
    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}