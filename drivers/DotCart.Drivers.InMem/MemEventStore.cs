using System.Collections;
using System.Collections.Immutable;
using DotCart.Behavior;
using DotCart.Effects;
using DotCart.Schema;

namespace DotCart.Drivers.InMem;

public interface IMemEventStore : IEventStore
{
    IEnumerable GetStream(IID engineId);
}


internal class MemEventStore : IMemEventStore
{
    private IImmutableDictionary<string, IEnumerable<IEvt>?> Streams = ImmutableSortedDictionary<string, IEnumerable<IEvt>?>.Empty;
    public bool IsClosed { get; private set; }


    public void Close()
    {
        IsClosed = true;
    }

    public void Load(IAggregate aggregate)
    {
        IEnumerable<IEvt>? evts = new List<IEvt>();
        if (Streams.TryGetValue(aggregate.Id(), out evts)) aggregate.Load(evts);
    }

    public void Save(IAggregate aggregate)
    {
        Streams = Streams.SetItem(aggregate.Id(), aggregate.UncommittedEvents.ToList() );
        aggregate.ClearUncommittedEvents();
    }

    public IEnumerable GetStream(IID engineId)
    {
        return Streams[engineId.Value];
    }
}