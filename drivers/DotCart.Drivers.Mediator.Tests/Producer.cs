using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit.Schema;

namespace DotCart.Drivers.Mediator.Tests;

public class Producer : ActorB, IActor<Spoke>, IProducer
{
    public Producer(IExchange exchange) : base(exchange)
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
        return Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Task.Delay(2_000);
                await _exchange.Publish(TopicAtt.Get<TheMsg>(), TheMsg.Random, cancellationToken);
            }
        }, cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}