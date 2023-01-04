using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Serilog;

namespace DotCart.Drivers.Mediator.Tests;

[Name("NamedConsumer2")]
public class NamedConsumer2 : ActorB, IActorT<TheSpoke>, INamedConsumer
{
    private readonly IExchange _exchange;

    public NamedConsumer2(IExchange exchange) : base(exchange)
    {
        _exchange = exchange;
    }

    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>)Task.CompletedTask;
    }

    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            Log.Information($"{Name} is subscribing to {_exchange.GetType()}");
            _exchange.Subscribe(TopicAtt.Get<TheSchema.Msg>(), this);
        }, cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            Log.Information($"{Name} is unsubscribing from {_exchange.GetType()}");
            _exchange.Unsubscribe(TopicAtt.Get<TheSchema.Msg>(), this);
        }, cancellationToken);
    }
}