using DotCart.Contract.Dtos;

namespace DotCart.Context.Effects;

public interface IReactor
{
    bool IsRunning { get; }
    Task StartAsync(CancellationToken cancellationToken);
    Task HandleAsync(IMsg msg, CancellationToken cancellationToken);
    void SetSpoke(ISpoke spoke);
}