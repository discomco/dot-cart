using DotCart.Schema;

namespace DotCart.Effects;

public interface IStore<TState> : IDisposable where TState: IState
{
    Task<TState> SetAsync(TState entity);
    Task<bool> DeleteAsync(string id);
    Task<bool> Exists(string id);
    Task<TState> GetByIdAsync(string id);
}