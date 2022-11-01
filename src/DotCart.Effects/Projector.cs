using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Effects.Drivers;


namespace DotCart.Effects;

public class Projector : Reactor, IProjector
{
    private readonly ITopicMediator _mediator;
    private readonly IProjectorDriver _projectorDriver;

    public Projector(
        ITopicMediator mediator, 
        IProjectorDriver projectorDriver)
    {
        _mediator = mediator;
        _projectorDriver = projectorDriver;
    }


    protected override Task StartReactingAsync(CancellationToken cancellationToken)
    {
        return _projectorDriver.StartStreamingAsync(cancellationToken);
    }

    protected override Task StopReactingAsync(CancellationToken cancellationToken)
    {
        return _projectorDriver.StopStreamingAsync(cancellationToken);
    }

    public override Task HandleAsync(IMsg msg)
    {
        return _mediator.PublishAsync(msg.MsgType, (IEvt)msg);
    }
}