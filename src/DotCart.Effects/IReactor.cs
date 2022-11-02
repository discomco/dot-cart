using DotCart.Contract;

namespace DotCart.Effects;

public interface IReactor
{
    Task StartAsync(CancellationToken cancellationToken);
    Task HandleAsync(IMsg msg);
    void SetSpoke(ISpoke spoke);    
}

