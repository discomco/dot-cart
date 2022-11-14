using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Contract.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DotCart.Context.Effects;

public static partial class Inject
{
    public static IServiceCollection AddProjector(this IServiceCollection services)
    {
        return services
            .AddTransient<IActor, Projector>();
    }
}

public class Projector : Actor, IProjector, IProducer
{
    private readonly IProjectorDriver _projectorDriver;

    public Projector(
        IExchange exchange,
        IProjectorDriver projectorDriver) : base(exchange)
    {
        _projectorDriver = projectorDriver;
    }

    public override async Task HandleCast(IMsg msg, CancellationToken cancellationToken)
    {
        Log.Information(msg is IEvt evt
            ? $"[{Name}] ~> {evt.Topic} @ {evt.AggregateID.Id()}"
            : $"[{Name}] ~> {msg.Topic}");
        await _exchange.Publish(msg.Topic, (IEvt)msg, cancellationToken);
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>?)Task.CompletedTask;
    }

    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            _projectorDriver.SetActor(this);
            Log.Information("Projector :: has Started.");
            return _projectorDriver.StartStreamingAsync(cancellationToken);
        }, cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            Log.Information("Projector :: is Stopping.");
            return _projectorDriver.StopStreamingAsync(cancellationToken);
        }, cancellationToken);
    }
}