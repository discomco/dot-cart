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
            .AddConfiguredESDBClients()
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
        _client.Dispose();
    }

    public void SetReactor(IReactor reactor)
    {
        _reactor = reactor;
    }

    public void Close()
    {

    }

    public async Task LoadAsync(IAggregate aggregate)
    {
        // var streamId = aggregate.ID.Value;
        // var events = _client.ReadStreamAsync(Direction.Forwards, streamId, StreamPosition.Start);
        // if (await events.ReadState == ReadState.StreamNotFound) return;
        // await foreach (var evt in events)
        // {
        //     evt.Event.
        //     
        // }

    }

    public Task SaveAsync(IAggregate aggregate)
    {
        var res = AppendEventsAsync(aggregate.ID, aggregate.UncommittedEvents);
        aggregate.ClearUncommittedEvents();
        return res;
    }
    
    

    public async Task<IEnumerable<IEvt>> ReadEventsAsync(IID ID)
    {
        var ret = new List<IEvt>();
        var readResult = _client.ReadStreamAsync(Direction.Forwards, ID.Value, StreamPosition.Start);
        var state = await readResult.ReadState.ConfigureAwait(false);
        if (state == ReadState.StreamNotFound) return ret;
        return await GetStoreEventsAsync(readResult);
    }

    private static async Task<IEnumerable<IEvt>> GetStoreEventsAsync(EventStoreClient.ReadStreamResult readResult)
    {
        var res = new List<IEvt>();
        await foreach (var evt in readResult)
        {
            var e = SerializationHelper.Deserialize(evt.Event.EventType, evt.Event.Data.ToArray());
            e.SetVersion(evt.Event.EventNumber.ToInt64());
            res.Add(e);
        }
        return res;
    } 




    public async Task<AppendResult> AppendEventsAsync(IID ID, IEnumerable<IEvt> events)
    {
        var storeEvents = new List<EventData>();
        storeEvents = events.Aggregate(storeEvents, (list, evt) =>
        {
            list.Add(Evt2EventData(evt));
            return list;
        });
        var writeResult = await _client.AppendToStreamAsync(ID.Value, StreamState.Any, storeEvents);
        return AppendResult.New(writeResult.NextExpectedStreamRevision.ToUInt64());
    }

    
    private EventData Evt2EventData(IEvt evt)
    {
        var eventId = Uuid.FromGuid(Guid.Parse(evt.MsgId));
        var typeName = evt.GetType().AssemblyQualifiedName;
        ReadOnlyMemory<byte> metaData = evt.MetaData;
        ReadOnlyMemory<byte> data = evt.Data;
        return new EventData(eventId, typeName, data, metaData);
    }
    
    
}