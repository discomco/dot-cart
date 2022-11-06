using System.Collections.Immutable;
using DotCart.Behavior;
using DotCart.Effects;
using DotCart.Effects.Drivers;
using DotCart.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.InMem;

public interface IMemEventStoreDriver : IEventStoreDriver
{
    IEnumerable<IEvt> GetStream(IID engineId);
}

public static partial class Inject
{
    public static IServiceCollection AddMemEventStore(this IServiceCollection services)
    {
        return services
            .AddTopicMediator()
            .AddMemProjector()
            .AddSingleton<IMemEventStoreDriver, MemEventStoreDriver>()
            .AddSingleton<IAggregateStoreDriver, MemEventStoreDriver>();
    }
}

internal class MemEventStoreDriver : IMemEventStoreDriver
{
    private readonly IMemProjector _projector;

    private readonly object loadMutex = new();


    private readonly object saveMutex = new();

    private IReactor _reactor;


    private IImmutableDictionary<string, IEnumerable<IEvt>?> Streams =
        ImmutableSortedDictionary<string, IEnumerable<IEvt>?>.Empty;


    public MemEventStoreDriver(IMemProjector projector)
    {
        _projector = projector;
    }

    public bool IsClosed { get; private set; }


    public void Close()
    {
        IsClosed = true;
    }

    public Task LoadAsync(IAggregate aggregate)
    {
        return Task.Run(() =>
        {
            lock (loadMutex)
            {
                IEnumerable<IEvt>? evts = new List<IEvt>();
                if (Streams.TryGetValue(aggregate.Id(), out evts))
                    aggregate.Load(evts);
            }
        });
    }

    public async Task SaveAsync(IAggregate aggregate)
    {
        Streams = Streams.SetItem(aggregate.Id(), aggregate.UncommittedEvents.ToList());
        aggregate.ClearUncommittedEvents();
        foreach (var evt in GetStream(aggregate.ID))
            await _projector.Project(evt);
    }

    public IEnumerable<IEvt> GetStream(IID engineId)
    {
        return Streams[engineId.Value];
    }

    public void Dispose()
    {
        
    }

    public void SetReactor(IReactor reactor)
    {
        _reactor = reactor;
    }

    public Task<IEnumerable<IEvt>> ReadEventsAsync(IID ID)
    {
        throw new NotImplementedException();
    }

    public Task<AppendResult> AppendEventsAsync(IID ID, IEnumerable<IEvt> events)
    {
        throw new NotImplementedException();
    }
}