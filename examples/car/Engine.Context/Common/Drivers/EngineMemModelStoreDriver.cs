using DotCart.Context.Effects.Drivers;
using DotCart.Drivers.InMem;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Common.Drivers;

public static partial class Inject
{
    public static IServiceCollection AddEngineMemStore(this IServiceCollection services)
    {
        return services
            .AddSingleton<IModelStoreDriver<Schema.Engine>, EngineMemModelStoreDriver>()
            .AddSingleton<IEngineModelStoreDriver, EngineMemModelStoreDriver>();
    }
}

public interface IEngineModelStoreDriver : IModelStoreDriver<Schema.Engine>
{
}

public class EngineMemModelStoreDriver : MemModelStoreDriver<Schema.Engine>, IEngineModelStoreDriver
{
}