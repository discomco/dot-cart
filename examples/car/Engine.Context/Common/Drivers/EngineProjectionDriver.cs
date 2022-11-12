using DotCart.Context.Drivers;
using DotCart.Context.Effects.Drivers;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Common.Drivers;


public class EngineProjectionDriver : ProjectionDriver<Schema.Engine>, IEngineMemProjectionDriver
{
    public EngineProjectionDriver(IModelStoreDriver<Schema.Engine> modelStoreDriver) : base(modelStoreDriver)
    {
    }
}

public interface IEngineMemProjectionDriver : IProjectionDriver<Schema.Engine>
{
}