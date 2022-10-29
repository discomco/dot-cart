using DotCart.Behavior;
using DotCart.Effects;

namespace DotCart.Drivers.InMem;

internal class MemEventStore : IEventStore
{
    private IDictionary<string, IEnumerable<IEvt>?> Streams = new Dictionary<string, IEnumerable<IEvt>?>();
    public bool IsClosed { get; private set; }


    public void Close()
    {
        IsClosed = true;
    }

    public void Load(IAggregate aggregate)
    {
        IEnumerable<IEvt>? evts = new List<IEvt>();
        if (Streams.TryGetValue(aggregate.Id(), out evts))
        {
            aggregate.Load(evts);
        }
    }

    public void Save(IAggregate aggregate)
    {
        Streams[aggregate.Id()] = aggregate.UncommittedEvents;
        aggregate.ClearUncommittedEvents();
    }
}