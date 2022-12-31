using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using static System.Threading.Tasks.Task;

namespace DotCart.Abstractions.Actors;

public abstract class ActorB : ActiveComponent, IActor
{
    protected readonly IExchange _exchange;
    private ISpokeB _spoke;
    protected IDriverB Driver;

    protected ActorB(IExchange exchange)
    {
        _exchange = exchange;
    }

    public abstract Task HandleCast(IMsg msg, CancellationToken cancellationToken = default);
    public abstract Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default);

    protected override Task PrepareAsync(CancellationToken cancellationToken = default)
    {
        return Run(async () =>
        {
            if (Driver != null) Driver.SetActor(this);
            while (_exchange.Status != ComponentStatus.Active)
            {
                _exchange.Activate(cancellationToken);
                await Delay(10, cancellationToken);
            }

            return CompletedTask;
        }, cancellationToken);
    }

    public void SetSpoke(ISpokeB spoke)
    {
        _spoke = spoke;
    }
}