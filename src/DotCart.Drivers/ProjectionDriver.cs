using DotCart.Effects;
using DotCart.Effects.Drivers;
using DotCart.Schema;

namespace DotCart.Drivers;

public abstract class ProjectionDriver<TReadModel> : IProjectionDriver<TReadModel> where TReadModel : IState
{
    private readonly IStore<TReadModel> _store;
    private IReactor _reactor;

    protected ProjectionDriver(IStore<TReadModel> store)
    {
        _store = store;
    }

    public void SetReactor(IReactor reactor)
    {
        _reactor = reactor;
    }

    public Task<TReadModel> GetByIdAsync(string id)
    {
        return _store.GetByIdAsync(id);
    }

    public Task<TReadModel> SetAsync(string id, TReadModel state)
    {
        return _store.SetAsync(id, state);
    }
}