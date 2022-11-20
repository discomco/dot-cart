using System.Collections.Immutable;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Drivers.Mediator;
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
            .AddSingletonExchange()
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

    private IActor _actor;


    private IImmutableDictionary<string, IEnumerable<IEvt>?> Streams =
        ImmutableSortedDictionary<string, IEnumerable<IEvt>?>.Empty;


    public MemEventStoreDriver(IMemProjector projector)
    {
        _projector = projector;
    }

    public bool IsClosed { get; private set; }

    public IEnumerable<IEvt> GetStream(IID engineId)
    {
        return Streams[engineId.Id()];
    }


    public void Close()
    {
        IsClosed = true;
    }

    public void Dispose()
    {
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


    public async Task<AppendResult> SaveAsync(IAggregate aggregate, CancellationToken cancellationToken = default)
    {
        Streams = Streams.SetItem(aggregate.Id(), aggregate.UncommittedEvents.ToList());
        aggregate.ClearUncommittedEvents();
        var evts = GetStream(aggregate.ID).ToArray();
        foreach (var evt in evts)
            await _projector.HandleCast(evt, cancellationToken);
        return AppendResult.New((ulong)(evts.Length + 1));
    }

    public void SetActor(IActor actor)
    {
        _actor = actor;
    }


    public Task<IEnumerable<IEvt>> ReadEventsAsync(IID ID, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<AppendResult> AppendEventsAsync(IID ID, IEnumerable<IEvt> events,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}