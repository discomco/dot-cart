using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.Schema;

namespace DotCart.Actors;

public class ResponderT<TSpoke, TPayload, TPipeInfo> : ActorT<TSpoke>, IResponderT<TPayload>
    where TPayload : IPayload
    where TSpoke : ISpokeT<TSpoke>
    where TPipeInfo : IPipeInfoB
{
    private readonly IPipeBuilderT<TPipeInfo, TPayload> _builder;

    private IPipeT<TPipeInfo, TPayload> _pipe;

    public ResponderT(
        IResponderDriverT<TPayload> driver,
        IExchange exchange,
        IPipeBuilderT<TPipeInfo, TPayload> builder) : base(exchange)
    {
        Driver = driver;
        _builder = builder;
    }


    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public override async Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        var hope = (HopeT<TPayload>)msg;
        _pipe = _builder.Build();
        return await _pipe.ExecuteAsync(hope, cancellationToken);
    }

    protected override string GetName()
    {
        return $"{HopeTopicAtt.Get<TPayload>()} ~> {Driver.GetType().Name}";
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