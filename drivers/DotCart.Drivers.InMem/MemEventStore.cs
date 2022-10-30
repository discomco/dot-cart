using System.Collections;
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
            .AddSingleton<IAggregateStore, MemEventStore>()
            .AddSingleton<IMemEventStore, MemEventStore>();
    }
}

internal class MemEventStore : IMemEventStore
{


    public MemEventStore(ITopicMediator mediator)
    {
        _mediator = mediator;
    }
    
    
    private IImmutableDictionary<string, IEnumerable<IEvt>?> Streams = ImmutableSortedDictionary<string, IEnumerable<IEvt>?>.Empty;
    
    private readonly ITopicMediator _mediator;
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
        if (_mediator == null) return;
        foreach (var evt in GetStream(aggregate.ID))
        {
            _mediator.PublishAsync(evt.MsgType, evt);
        }
    }

    public IEnumerable<IEvt> GetStream(IID engineId)
    {
        return Streams[engineId.Value];
    }
}