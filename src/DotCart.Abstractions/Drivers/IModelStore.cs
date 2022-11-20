using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IModelStore<TState> : IClose, IDisposable, IAsyncDisposable, ICloseAsync
    where TState : IState
{
    Task<TState> SetAsync(string id, TState doc, CancellationToken cancellationToken = default);
    Task<TState> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> Exists(string id, CancellationToken cancellationToken = default);
    Task<TState?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> HasData(CancellationToken cancellationToken = default);
}