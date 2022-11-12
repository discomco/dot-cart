using DotCart.Context.Behaviors;
using DotCart.Context.Effects.Drivers;
using DotCart.Contract.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DotCart.Context.Effects;

public static partial class Inject
{
    public static IServiceCollection AddProjector<TSpoke>(this IServiceCollection services) where TSpoke : ISpoke<TSpoke>
    {
        return services
            .AddTransient<IProjector<TSpoke>, Projector<TSpoke>>();
    }
}

public interface IProjector<in TSpoke> : IReactor<TSpoke> where TSpoke : ISpoke<TSpoke>
{
}

public class Projector<TSpoke> : Reactor<TSpoke>, IProjector<TSpoke> 
    where TSpoke : ISpoke<TSpoke>
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

    public override Task HandleAsync(IMsg msg, CancellationToken cancellationToken)
    {
        Log.Information($"Projector :: Publishing Message: {msg.Topic}");
        return _mediator.PublishAsync(msg.Topic, (IEvt)msg);
    }

    protected override Task StartReactingAsync(CancellationToken cancellationToken)
    {
        _projectorDriver.SetReactor(this);
        Log.Information("Projector :: has Started.");
        return _projectorDriver.StartStreamingAsync(cancellationToken);
    }

    protected override Task StopReactingAsync(CancellationToken cancellationToken)
    {
        Log.Information("Projector :: is Stopping.");
        return _projectorDriver.StopStreamingAsync(cancellationToken);
    }
}