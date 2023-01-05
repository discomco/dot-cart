using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.Core;
using DotCart.TestKit;
using Serilog;

namespace DotCart.Context.Tests.Actors;

public interface IConsumer1 : IActor
{
}

[Name("Consumer1")]
public class Consumer1 : ActorB, IActorT<TheSpoke>, IConsumer1
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