using DotCart.Schema;

namespace DotCart.Effects;

public interface IProjectionDriver<TState> : IDriver
    where TState : IState
{
    Task<TState> GetByIdAsync(string id); 
    Task<TState> SetAsync(string id, TState state);
}