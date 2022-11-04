using DotCart.Drivers.InMem;
using DotCart.Effects;
using DotCart.Effects.Drivers;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestEnv.Engine.Effects;

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