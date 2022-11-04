using DotCart.Schema;

namespace DotCart.Effects.Drivers;

public interface IModelStoreDriver<TState> :
    IClose,
    IDisposable 
    where TState : IState
{
    Task<TState> SetAsync(string id, TState doc);
    Task<bool> DeleteAsync(string id);
    Task<bool> Exists(string id);
    Task<TState?> GetByIdAsync(string id);
}