using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit.Schema;
using Serilog;

namespace DotCart.Drivers.Mediator.Tests;

public interface IConsumer1
{
}

public class Consumer1 : ActorB, IActor<Spoke>, IConsumer1
{
    public Consumer1(IExchange exchange) : base(exchange)
    {
    }


    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken)
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
}