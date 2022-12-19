using System.Text;
using DotCart.Abstractions.Behavior;
using Newtonsoft.Json;

namespace DotCart.Drivers.EventStoreDB;

public static class SerializationHelper
{
    public static IEvtB Deserialize(string eventType, byte[] data)
    {
        try
        {
            var res = (IEvtB)Activator.CreateInstance(Type.GetType(eventType));
            res.SetData(data);

            // var settings = new JsonSerializerSettings
            // {
            //     ContractResolver = new PrivateSetterContractResolver()
            // };
            //
            // var des = JsonConvert.DeserializeObject(
            //     Encoding.UTF8.GetString(data),
            //     Type.GetType(eventType), 
            //     settings)!;

            return res;

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

    public static byte[] Serialize(IEvtB @event)
    {
        return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
        //return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
        //return JsonSerializer.SerializeToUtf8Bytes(@event);
    }
}