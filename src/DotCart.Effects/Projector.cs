using DotCart.Behavior;
using Microsoft.Extensions.Hosting;

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
    

    public Task Project(IEvt evt)
    {
        throw new NotImplementedException();
    }

    protected override Task StartReactingAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    protected override Task StopReactingAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}