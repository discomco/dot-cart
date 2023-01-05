using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Context.Actors;

public class ListenerT<TSpoke, TCmdPayload, TMeta, TFactPayload, TDriverMsg>
    : ActorB, ISubscriber
    where TCmdPayload : IPayload
    where TMeta : IEventMeta
    where TFactPayload : IPayload
    where TSpoke: ISpokeT<TSpoke>
    where TDriverMsg : class
{
    private readonly Fact2Cmd<TCmdPayload, TMeta, TFactPayload> _fact2Cmd;
    private readonly ISequenceBuilderT<TFactPayload> _sequenceBuilder;

    protected ListenerT(
        IListenerDriverB driver,
        IExchange exchange,
        ISequenceBuilderT<TFactPayload> sequenceBuilder) : base(exchange)
    {
        Driver = driver;
        _sequenceBuilder = sequenceBuilder;
    }

    public override async Task HandleCast(IMsg msg, CancellationToken cancellationToken = default)
    {
        var fact = (FactT<TFactPayload, TMeta>)msg;
        var sequence = _sequenceBuilder.Build();
        var fdbk = await sequence.ExecuteAsync(fact, cancellationToken);
    }

    private string GetSequenceName()
    {
        return $"{NameAtt.Get<TSpoke>()}:listener_seq";
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
        return ((IListenerDriverT<TFactPayload, TDriverMsg>)Driver).StartListeningAsync(cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken = default)
    {
        return ((IListenerDriverT<TFactPayload, TDriverMsg>)Driver).StopListeningAsync(cancellationToken);
    }
}