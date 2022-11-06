using DotCart.Behavior;
using DotCart.Drivers.EventStoreDB.Interfaces;
using DotCart.Effects;
using DotCart.Effects.Drivers;
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
        throw new NotImplementedException();
    }
    
    
    // private IEvt Deserialize<TAggregateId>(byte[] data)
    // {
    //     try
    //     {
    //         var settings = new JsonSerializerSettings {ContractResolver = new PrivateSetterContractResolver()};
    //         return (IEvt) JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data),
    //             typeof(TEvent), settings);
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         throw new EventStoreDeserializationException($"Failed to Deserialize eventTyp {typeof(TEvent)}", data, e);
    //     }
    // }
    //
    // private byte[] Serialize<TAggregateId>(IEvent<TAggregateId> @event)
    // {
    //     return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
    // }


    
    
    
}