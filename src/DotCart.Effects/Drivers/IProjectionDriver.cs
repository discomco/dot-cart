using DotCart.Schema;

namespace DotCart.Effects.Drivers;

public interface IProjectionDriver<TState> : IModelStoreDriver<TState>
    where TState : IState
{
    Task<TState> GetByIdAsync(string id);
    Task<TState> SetAsync(string id, TState state);
}