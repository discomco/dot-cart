using System.Collections.Immutable;
using DotCart.Behavior;
using DotCart.Effects;
using DotCart.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.InMem;

public interface IMemEventStore : IEventStore
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
            .AddSingleton<IAggregateStore, MemEventStore>()
            .AddSingleton<IMemEventStore, MemEventStore>();
    }
}

internal class MemEventStore : IMemEventStore
{
    private readonly IMemProjector _projector;


    private IImmutableDictionary<string, IEnumerable<IEvt>?> Streams =
        ImmutableSortedDictionary<string, IEnumerable<IEvt>?>.Empty;


    public MemEventStore(IMemProjector projector)
    {
        _projector = projector;
    }

    public bool IsClosed { get; private set; }


    public void Close()
    {
        IsClosed = true;
    }

    private readonly object loadMutex = new();

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


    private readonly object saveMutex = new();

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
}