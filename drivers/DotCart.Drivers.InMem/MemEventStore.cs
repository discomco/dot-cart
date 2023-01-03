using System.Collections.Immutable;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using Microsoft.Extensions.DependencyInjection;
using static System.Threading.Tasks.Task;

namespace DotCart.Drivers.InMem;

public interface IMemEventStore : IEventStore
{
    IEnumerable<IEvtB> GetStream(IID engineId);
}

public static partial class Inject
{
    public static IServiceCollection AddMemEventStore(this IServiceCollection services)
    {
        return services
            .AddSingletonExchange()
            .AddMemProjector()
            .AddSingleton<IEventStore, MemEventStore>()
            .AddSingleton<IMemEventStore, MemEventStore>()
            .AddSingleton<IAggregateStore, MemEventStore>();
    }
}

internal class MemEventStore : IMemEventStore
{
    private readonly IMemProjector _projector;

    private readonly object loadMutex = new();


    private readonly object saveMutex = new();

    private IActor _actor;


    private IImmutableDictionary<string, IEnumerable<IEvtB>?> Streams =
        ImmutableSortedDictionary<string, IEnumerable<IEvtB>?>.Empty;


    public MemEventStore(IMemProjector projector)
    {
        _projector = projector;
    }

    public bool IsClosed { get; private set; }

    public IEnumerable<IEvtB> GetStream(IID engineId)
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
                IEnumerable<IEvtB>? evts = new List<IEvtB>();
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


    public Task<IEnumerable<IEvtB>> ReadEventsAsync(IID ID, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<AppendResult> AppendEventsAsync(IID ID, IEnumerable<IEvtB> events,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}