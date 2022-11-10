using DotCart.Effects;
using DotCart.Effects.Drivers;
using DotCart.Schema;

namespace DotCart.Drivers;

public abstract class ProjectionDriver<TReadModel> : IProjectionDriver<TReadModel> where TReadModel : IState
{
    private readonly IModelStoreDriver<TReadModel> _modelStoreDriver;
    
    private IReactor _reactor;

    protected ProjectionDriver(IModelStoreDriver<TReadModel> modelStoreDriver)
    {
        _modelStoreDriver = modelStoreDriver;
    }

    public void SetReactor(IReactor reactor)
    {
        _reactor = reactor;
    }

    public Task<bool> Exists(string id)
    {
        throw new NotImplementedException();
    }

    public Task<TReadModel> GetByIdAsync(string id)
    {
        return _modelStoreDriver.GetByIdAsync(id);
    }

    public Task<bool> HasData()
    {
        return _modelStoreDriver.HasData();
    }

    public Task<TReadModel> SetAsync(string id, TReadModel state)
    {
        return _modelStoreDriver.SetAsync(id, state);
    }

    public Task<bool> DeleteAsync(string id)
    {
        return _modelStoreDriver.DeleteAsync(id);
    }

    public void Close()
    {
        _modelStoreDriver.Close();
    }

    public void Dispose()
    {
        _modelStoreDriver.Dispose();
    }
}