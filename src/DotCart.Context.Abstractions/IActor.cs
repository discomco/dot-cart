using DotCart.Contract.Dtos;

namespace DotCart.Context.Abstractions;

public interface IActor<in TSpoke> : IActor
{
    void SetSpoke(ISpoke spoke);
}

public interface IActor<in TSpoke, TActor> : IActor<TSpoke>
    where TActor : IActor
    where TSpoke : ISpokeT<TSpoke>
{
}

public interface IActor : IActiveComponent
{
    Task HandleCast(IMsg msg, CancellationToken cancellationToken = default);
    Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default);
}

public interface IActiveComponent
{
    string Name { get; }
    bool IsRunning { get; }
    Task Activate(CancellationToken cancellationToken = default);
}