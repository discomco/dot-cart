using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Effects.Drivers;
using static System.Threading.Tasks.Task;


namespace DotCart.Effects;

public delegate TFact Evt2Fact<out TFact>(IEvt evt) where TFact : IFact;

public abstract class Emitter<TDriver, TEvt, TFact> : Reactor, IEmitter
    where TDriver : IEmitterDriver
    where TEvt : IEvt
    where TFact : IFact
{
    private readonly ITopicMediator _mediator;
    private readonly Evt2Fact<TFact> _evt2Fact;
    private readonly IEmitterDriver _emitterDriver;

    public Emitter(
        IEmitterDriver emitterDriver,
        ITopicMediator mediator,
        Evt2Fact<TFact> evt2Fact)
    {
        _mediator = mediator;
        _evt2Fact = evt2Fact;
        _emitterDriver = emitterDriver;
    }

    protected override Task StartReactingAsync(CancellationToken cancellationToken)
    {
        return Run(() => { _mediator.Subscribe(Topic.Get<TEvt>(), Handler);}, cancellationToken);
        
    }

    private Task Handler(IEvt evt)
    {
        return Run(() =>
        {
            var fact = _evt2Fact(evt);
            _emitterDriver.EmitFact(fact);
        });
    }

    protected override Task StopReactingAsync(CancellationToken cancellationToken)
    {
        return Run(() => { _mediator.UnsubscribeAsync(Topic.Get<TEvt>(), Handler ); }, cancellationToken);
    }

    public void SetSpoke(ISpoke spoke)
    {
        spoke.Inject(this);
    }

    public override Task HandleAsync(IMsg msg)
    {
        return CompletedTask;
    }

}