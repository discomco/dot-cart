using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Effects.Drivers;
using static System.Threading.Tasks.Task;


namespace DotCart.Effects;

public delegate TFact Evt2Fact<out TFact, in TEvt>(Event evt) where TFact : IFact where TEvt: IEvt;

public interface IEmitter : IReactor
{
}

public abstract class Emitter<TDriver, TEvt, TFact> : Reactor, IEmitter
    where TDriver : IEmitterDriver
    where TEvt : IEvt
    where TFact : IFact
{
    private readonly IEmitterDriver _emitterDriver;
    private readonly Evt2Fact<TFact, TEvt> _evt2Fact;
    private readonly ITopicMediator _mediator;

    protected Emitter(
        IEmitterDriver emitterDriver,
        ITopicMediator mediator,
        Evt2Fact<TFact, TEvt> evt2Fact)
    {
        _mediator = mediator;
        _evt2Fact = evt2Fact;
        _emitterDriver = emitterDriver;
    }


    public override Task HandleAsync(IMsg msg, CancellationToken cancellationToken)
    {
        return CompletedTask;
    }

    protected override Task StartReactingAsync(CancellationToken cancellationToken)
    {
        return Run(() => { _mediator.SubscribeAsync(Topic.Get<TEvt>(), Handler); }, cancellationToken);
    }

    private Task Handler(IEvt evt)
    {
        return Run(() =>
        {
            var fact = _evt2Fact((Event)evt);
            _emitterDriver.EmitFact(fact);
        });
    }

    protected override Task StopReactingAsync(CancellationToken cancellationToken)
    {
        return Run(() => { _mediator.UnsubscribeAsync(Topic.Get<TEvt>(), Handler); }, cancellationToken);
    }
}