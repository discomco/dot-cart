using DotCart.Context.Abstractions;
using DotCart.Context.Effects;
using DotCart.Contract.Dtos;
using DotCart.Core;
using DotCart.TestKit;
using Serilog;

namespace DotCart.Drivers.Mediator.Tests;

public class Consumer1 : ActorT<Spoke>, IActor<ISpokeT<Spoke>, Consumer1>, IConsumer
{
    public Consumer1(IExchange exchange) : base(exchange)
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