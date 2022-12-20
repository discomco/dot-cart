using DotCart.Abstractions.Behavior;
using EventStore.Client;

namespace DotCart.Drivers.EventStoreDB;

public static class Conversions
{
    public static EventData ToEventData(this IEvtB evt)
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

    // public static Event ToEvent<TID>(this EventRecord rEvt, 
    //     IDCtorT<TID> idCtor) 
    //     where TID : IID
    // {
    //     var id = rEvt.EventStreamId.IDFromIdString(idCtor);
    //     var evt = Event.New(id, rEvt.EventType, rEvt.Data.ToArray(), rEvt.Metadata.ToArray());
    //     evt.SetVersion(rEvt.EventNumber.ToInt64());
    //     evt.SetTimeStamp(rEvt.Created);
    //     return evt;
    // }

    public static Event ToEvent(this EventRecord rEvt)
    {
        if (rEvt == null) return null;
        var res = new Event(
            rEvt.EventStreamId,
            rEvt.EventType,
            rEvt.Data.ToArray(),
            rEvt.Metadata.ToArray());
        res.SetVersion(rEvt.EventNumber.ToInt64());
        res.SetTimeStamp(rEvt.Created);
        return res;
    }
}