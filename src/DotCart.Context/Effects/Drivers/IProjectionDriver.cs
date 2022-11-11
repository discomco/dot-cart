using DotCart.Context.Schemas;

namespace DotCart.Context.Effects.Drivers;

public interface IProjectionDriver : IClose, IDisposable
{
}

public interface IProjectionDriver<TState> : IProjectionDriver
    where TState : IState
{
    Task<TState> SetAsync(string id, TState doc);
    Task<bool> DeleteAsync(string id);
    Task<bool> Exists(string id);
    Task<TState?> GetByIdAsync(string id);
    Task<bool> HasData();
}