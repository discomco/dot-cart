using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit.Schema;
using Serilog;

namespace DotCart.Drivers.Mediator.Tests;

[Name("NamedConsumer1")]
public class NamedConsumer1 : ActorB, IActor<TheSpoke>, INamedConsumer
{
    private readonly IExchange _exchange;

    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            Log.Information($"{AppVerbs.Subscribing} [{NameAtt.Get(this)}] ~> [{_exchange.GetType()}]");
            _exchange.Subscribe(TopicAtt.Get<TheMsg>(), this);
        }, cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            Log.Information($"{Name} is unsubscribing from {_exchange.GetType()}");
            _exchange.Unsubscribe(TopicAtt.Get<TheMsg>(), this);
        }, cancellationToken);
    }

    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>)Task.CompletedTask;
    }

    public NamedConsumer1(IExchange exchange) : base(exchange)
    {
        _exchange = exchange;
    }
}