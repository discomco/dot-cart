using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Effects.Drivers;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

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
    private readonly ILogger _logger;
    private readonly ITopicMediator _mediator;
    private readonly IProjectorDriver _projectorDriver;

    public Projector(
        ILogger logger,
        IProjectorDriver projectorDriver,
        ITopicMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
        _projectorDriver = projectorDriver;
    }

    public override Task HandleAsync(IMsg msg, CancellationToken cancellationToken)
    {
        _logger.Information($"Projector :: Publishing Message: {msg.Topic}");
        return _mediator.PublishAsync(msg.Topic, (IEvt)msg);
    }

    protected override Task StartReactingAsync(CancellationToken cancellationToken)
    {
        _projectorDriver.SetReactor(this);
        _logger.Information("Projector :: has Started.");
        return _projectorDriver.StartStreamingAsync(cancellationToken);
    }

    protected override Task StopReactingAsync(CancellationToken cancellationToken)
    {
        _logger.Information("Projector :: is Stopping.");
        return _projectorDriver.StopStreamingAsync(cancellationToken);
    }
}