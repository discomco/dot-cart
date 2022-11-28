using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.Drivers.EventStoreDB;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DotCart.Drivers.Default;

public static class Inject
{
    public static IServiceCollection AddSingletonProjector<TInfo>(this IServiceCollection services)
        where TInfo : ISubscriptionInfo
    {
        return services
            .AddConfiguredESDBClients()
            .AddSingletonESDBProjectorDriver<TInfo>()
            .AddSingleton<IProjector, Projector<TInfo>>();
    }

    public static IServiceCollection AddTransientProjector<TInfo>(this IServiceCollection services)
        where TInfo : ISubscriptionInfo
    {
        return services
            .AddTransientESDBProjectorDriver<TInfo>()
            .AddTransient<IProjector, Projector<TInfo>>();
    }
}

public class Projector<TInfo> : ActorB, IProjector, IProducer where TInfo : ISubscriptionInfo
{
    private readonly IProjectorDriverT<TInfo> _projectorDriver;

    public Projector(
        IExchange exchange,
        IProjectorDriverT<TInfo> projectorDriver) : base(exchange)
    {
        _projectorDriver = projectorDriver;
    }

    public override async Task HandleCast(IMsg msg, CancellationToken cancellationToken)
    {
        Log.Information(msg is IEvt evt
            ? $"[{Name}] ~> {TopicAtt.Get(evt)} @ {evt.AggregateID.Id()}"
            : $"[{Name}] ~> {TopicAtt.Get(msg)}");
        await _exchange.Publish( TopicAtt.Get(msg), (IEvt)msg, cancellationToken);
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