using DotCart.Contract.Dtos;

namespace DotCart.Context.Effects;

public interface IReactor
{
    bool IsRunning { get; }
    Task Activate(CancellationToken cancellationToken);

    Task HandleAsync(IMsg msg, CancellationToken cancellationToken);
//    void SetSpoke(ISpoke spoke);
}

public interface IReactor<in TSpoke> : IReactor
    where TSpoke : ISpoke<TSpoke>
{
    string Name { get; }
    void SetSpoke(TSpoke spoke);
}