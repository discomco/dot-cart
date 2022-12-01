using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using EventStore.Client;

namespace DotCart.Drivers.EventStoreDB;

public static class Conversions
{
    public static EventData ToEventData(this IEvt evt)
    {
        if (evt == null) return null;
        try
        {
            var eventId = Uuid.FromGuid(Guid.Parse(evt.EventId));
            var typeName = evt.Topic;
            ReadOnlyMemory<byte> metaData = evt.MetaData;
            ReadOnlyMemory<byte> data = evt.Data;
            return new EventData(eventId, typeName, data, metaData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static Event ToEvent(this EventRecord rEvt)
    {
        if (rEvt == null) return null;
        var res = Event.New(
            rEvt.EventStreamId.IDFromIdString(),
            rEvt.EventType,
            rEvt.Data.ToArray(),
            rEvt.Metadata.ToArray(),
            rEvt.EventNumber.ToInt64(),
            rEvt.Created);
        return res;
    }
}