using DotCart.Context.Abstractions;
using DotCart.Contract.Dtos;
using static System.Threading.Tasks.Task;

namespace DotCart.Context.Effects;

public abstract class Actor : ActiveComponent, IActor
{
    protected readonly IExchange _exchange;

    protected Actor(IExchange exchange)
    {
        _exchange = exchange;
    }

    public abstract Task HandleCast(IMsg msg, CancellationToken cancellationToken = default);
    public abstract Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default);


    protected override Task PrepareAsync(CancellationToken cancellationToken = default)
    {
        return Run(async () =>
        {
            while (_exchange.Status != ComponentStatus.Active)
            {
                _exchange.Activate(cancellationToken);
                await Delay(10, cancellationToken);
            }

            return CompletedTask;
        }, cancellationToken);
    }
}

public abstract class ActorT<TSpoke> : Actor, IActor<TSpoke> where TSpoke : ISpokeT<TSpoke>
{
    private ISpokeB _spoke;

    protected ActorT(IExchange exchange) : base(exchange)
    {
    }

    public void SetSpoke(ISpokeB spoke)
    {
        _spoke = spoke;
    }

    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return CompletedTask;
    }
}