using DotCart.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.InMem;

public static partial class Inject
{
    public static IServiceCollection AddMemProjector(this IServiceCollection services)
    {
        return services
            .AddTopicMediator()
            .AddSingleton<IProjector, MemProjector>()
            .AddSingleton<IMemProjector, MemProjector>();
    }
}

/// <summary>
///     MemProjector is an in-memory Unit of Effect,
///     that is meant to be injected into an AggregateStore
///     It offers a straightforward interface to project events onto the TopicMediator
/// </summary>
internal sealed class MemProjector : IMemProjector
{
    private readonly ITopicMediator _mediator;

    public MemProjector(ITopicMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Project(IEvt evt)
    {
        return _mediator.PublishAsync(evt.MsgType, evt);
    }
}

public interface IMemProjector : IProjector
{
}