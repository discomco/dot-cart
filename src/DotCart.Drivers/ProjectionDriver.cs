using DotCart.Effects;
using DotCart.Effects.Drivers;
using DotCart.Schema;

namespace DotCart.Drivers;

public abstract class ProjectionDriver<TState> : IProjectionDriver<TState> where TState : IState
{
    private readonly IStore<TState> _store;
    private IReactor _reactor;

    protected ProjectionDriver(IStore<TState> store)
    {
        _store = store;
    }

    public void SetReactor(IReactor reactor)
    {
        _reactor = reactor;
    }

    public Task<TState> GetByIdAsync(string id)
    {
        return _store.GetByIdAsync(id);
    }

    public Task<TState> SetAsync(string id, TState state)
    {
        return _store.SetAsync(id, state);
    }
};