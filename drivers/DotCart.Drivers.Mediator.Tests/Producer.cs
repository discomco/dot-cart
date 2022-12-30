using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;

namespace DotCart.Drivers.Mediator.Tests;

[Name("Producer")]
public class Producer : ActorB, IActor<TheSpoke>, IProducer
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
                await _exchange.Publish(TopicAtt.Get<TheSchema.Msg>(), TheSchema.Msg.Random, cancellationToken);
            }
        }, cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}