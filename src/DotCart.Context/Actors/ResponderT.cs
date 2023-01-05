using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Context.Actors;



public class ResponderT<TSpoke, TPayload, TMeta> : ActorT<TSpoke>, IResponderT<TPayload>
    where TMeta : IEventMeta
    where TPayload : IPayload
    where TSpoke : ISpokeT<TSpoke>
{
    private readonly ISequenceBuilderT<TPayload> _builder;

    private readonly Hope2Cmd<TPayload, TMeta> _hope2Cmd;

    private ISequenceT<TPayload> _sequence;
    //    private ICmdHandler _cmdHandler;
//    private readonly TDriver _responderDriver;

    public ResponderT(
        IResponderDriverT<TPayload> driver,
        IExchange exchange,
        ISequenceBuilderT<TPayload> builder,
        Hope2Cmd<TPayload, TMeta> hope2Cmd) : base(exchange)
    {
        Driver = driver;
        _builder = builder;
        _hope2Cmd = hope2Cmd;
    }


    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public override async Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        var hope = (HopeT<TPayload>)msg;
        var cmd = _hope2Cmd(hope);
        _sequence = _builder.Build();
        return await _sequence.ExecuteAsync(hope, cancellationToken);
    }

    private string GetSequenceName()
    {
        return $"{NameAtt.Get<TSpoke>()}:responder_seq";
    }

    protected override string GetName()
    {
        return $"{Driver.GetType().Name}<{HopeTopicAtt.Get<TPayload>()}>";
    }


    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken = default)
    {
        ((IResponderDriverT<TPayload>)Driver).SetActor(this);
        return ((IResponderDriverT<TPayload>)Driver).StartRespondingAsync(cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken = default)
    {
        return ((IResponderDriverT<TPayload>)Driver).StopRespondingAsync(cancellationToken);
    }
}