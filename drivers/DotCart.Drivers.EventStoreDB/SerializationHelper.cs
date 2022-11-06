using System.Text;
using DotCart.Behavior;
using EventStore.Client;
using Newtonsoft.Json;

namespace DotCart.Drivers.EventStoreDB;

public static class SerializationHelper
{
    public static IEvt Deserialize(string eventType, byte[] data)
    {
        try
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver()
            };
            
            var des = JsonConvert.DeserializeObject(
                Encoding.UTF8.GetString(data),
                Type.GetType(eventType), 
                settings)!;

            return (IEvt)des;

            // // var settings = new JsonSerializerSettings {ContractResolver = new PrivateSetterContractResolver()};
            // // return (IEvent<TAggregateId>) JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data),
            // //     Type.GetType(eventType), settings);
            //
            // // var settings = new JsonSerializerSettings {ContractResolver = new PrivateSetterContractResolver()};
            //     return (IEvent<TAggregateId>)JsonSerializer.Deserialize(data, Type.GetType(eventType));
            // //  JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data),
            // // Type.GetType(eventType), settings);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new EventStoreDeserializationException($"Failed to Deserialize eventType {eventType}", data, e);
        }
    }

    public static byte[] Serialize(IEvt @event)
    {
        return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
        //return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
        //return JsonSerializer.SerializeToUtf8Bytes(@event);
    }


    public static StoreEvent ToStoreEvent(this IEvt @event,
        long eventNumber)
    {
        return new StoreEvent(@event, eventNumber);
    }

    public static IEvt? Deserialize(ResolvedEvent resolvedEvent)
    {
        return Deserialize(resolvedEvent.Event.EventType, resolvedEvent.Event.Data.ToArray());
    }
}