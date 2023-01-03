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
        where TInfo : IProjectorInfoB
    {
        return services
            .AddConfiguredESDBClients()
            .AddSingletonESDBProjectorDriver<TInfo>()
            .AddSingleton<IProjector, Projector<TInfo>>();
    }

    public static IServiceCollection AddTransientProjector<TInfo>(this IServiceCollection services)
        where TInfo : IProjectorInfoB
    {
        return services
            .AddTransientESDBProjectorDriver<TInfo>()
            .AddTransient<IProjector, Projector<TInfo>>();
    }
}

[Name("dotcart:projector")]
public class Projector<TInfo>
    : ActorB, IProjector, IProducer
    where TInfo : IProjectorInfoB
{
//    private readonly IProjectorDriverT<TInfo> _projectorDriver;
    public Projector(
        IExchange exchange,
        IProjectorDriverT<TInfo> projectorDriver)
        : base(exchange)
    {
        Driver = projectorDriver;
    }

    public override async Task HandleCast(IMsg msg, CancellationToken cancellationToken)
    {
        if (msg is IEvtB evt)
        {
            Log.Information($"{AppVerbs.Projecting} {evt.Topic} ~> {evt.AggregateId}");
            evt.SetIsCommitted(true);
            await _exchange.Publish(evt.Topic, evt, cancellationToken);
        }
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
            Log.Information($"{AppFacts.Started} [Projector<{GroupNameAtt.Get<TInfo>()}-{IDPrefixAtt.Get<TInfo>()}>]");
            return ((IProjectorDriverT<TInfo>)Driver).StartStreamingAsync(cancellationToken);
        }, cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            Log.Information("Projector :: is Stopping.");
            return ((IProjectorDriverT<TInfo>)Driver).StopStreamingAsync(cancellationToken);
        }, cancellationToken);
    }
}