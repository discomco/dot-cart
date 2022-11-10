using System.Collections.Immutable;
using DotCart.Behavior;
using DotCart.Effects;
using DotCart.Effects.Drivers;
using DotCart.Schema;
using Microsoft.Extensions.DependencyInjection;
using static System.Threading.Tasks.Task;

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
            .AddSingleton<IEventStoreDriver, MemEventStoreDriver>()
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

    public Task LoadAsync(IAggregate aggregate, CancellationToken cancellationToken = default)
    {
        return Run(() =>
        {
            lock (loadMutex)
            {
                IEnumerable<IEvt>? evts = new List<IEvt>();
                if (Streams.TryGetValue(aggregate.Id(), out evts))
                    aggregate.Load(evts);
            }
        }, cancellationToken);
    }


    public async Task SaveAsync(IAggregate aggregate, CancellationToken cancellationToken = default)
    {
            Streams = Streams.SetItem(aggregate.Id(), aggregate.UncommittedEvents.ToList());
            aggregate.ClearUncommittedEvents();
            foreach (var evt in GetStream(aggregate.ID))
                await _projector.Project(evt, cancellationToken);
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


    public Task<IEnumerable<IEvt>> ReadEventsAsync(IID ID, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<AppendResult> AppendEventsAsync(IID ID, IEnumerable<IEvt> events, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}