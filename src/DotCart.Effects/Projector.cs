using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Effects.Drivers;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Effects;

public static partial class Inject
{
    public static IServiceCollection AddProjector(this IServiceCollection services)
    {
        return services
            .AddHostedService<Projector>();
    }
}

public interface IProjector : IReactor
{
}

public class Projector : Reactor, IProjector
{
    private readonly ITopicMediator _mediator;
    private readonly IProjectorDriver _projectorDriver;

    public Projector(
        IProjectorDriver projectorDriver,
        ITopicMediator mediator
    )
    {
        _mediator = mediator;
        _projectorDriver = projectorDriver;
    }

    public override Task HandleAsync(IMsg msg)
    {
        return _mediator.PublishAsync(msg.Topic, (IEvt)msg);
    }

    protected override Task StartReactingAsync(CancellationToken cancellationToken)
    {
        return _projectorDriver.StartStreamingAsync(cancellationToken);
    }

    protected override Task StopReactingAsync(CancellationToken cancellationToken)
    {
        return _projectorDriver.StopStreamingAsync(cancellationToken);
    }
}