using DotCart.Context.Abstractions;
using DotCart.Context.Effects;
using DotCart.Contract.Dtos;
using DotCart.Core;
using DotCart.TestKit;

namespace DotCart.Drivers.Mediator.Tests;

public class Producer : ActorT<Spoke>, IActor<ISpokeT<Spoke>, Producer>, IProducer
{
    public Producer(IExchange exchange) : base(exchange)
    {
    }

    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
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