using DotCart.Context.Effects.Drivers;
using DotCart.Drivers.InMem;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Common.Drivers;

public static partial class Inject
{

}

public interface IEngineModelStoreDriver : IModelStoreDriver<Schema.Engine>
{
}

public class EngineMemModelStoreDriver : MemModelStoreDriver<Schema.Engine>, IEngineModelStoreDriver
{
}