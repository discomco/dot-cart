using DotCart.Behavior;
using DotCart.Contract;
using Microsoft.Extensions.Hosting;

namespace DotCart.Effects;

public abstract class Emitter<TEvt, TFact> : BackgroundService, IEmitter
{

    public Emitter(
        ITopicMediator mediator, 
        IEmitterDriver driver)
    {
        
    }

    public void SetSpoke(ISpoke spoke)
    {
        
    }

    public Task Emit(IFact fact)
    {
        throw new NotImplementedException();
    }
}