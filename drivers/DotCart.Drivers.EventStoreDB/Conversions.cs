using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
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

    public static TEvt ToEvent<TEvt, TID>(this EventRecord rEvt, 
        EvtCtorT<TEvt, TID> evtCtor, 
        IDCtorT<TID> idCtor) 
        where TEvt : IEvtB
        where TID : IID
    {
        var id = rEvt.EventStreamId.IDFromIdString(idCtor);
        var evt = evtCtor(id);
        evt.SetData(rEvt.Data.ToArray());
        evt.SetMetaData(rEvt.Metadata.ToArray());
        evt.SetEventType(rEvt.EventType);
        evt.SetVersion(rEvt.EventNumber.ToInt64());
        evt.SetTimeStamp(rEvt.Created);
        return evt;
    }

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