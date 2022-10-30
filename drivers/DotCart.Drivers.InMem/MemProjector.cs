using DotCart.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.InMem;

public static partial class Inject
{
    public static IServiceCollection AddMemProjector(this IServiceCollection services)
    {
        return services
            .AddTopicMediator()
            .AddSingleton<IProjector, MemProjector>();
    }
}



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