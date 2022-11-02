using DotCart.Drivers.InMem;
using DotCart.Effects;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestEnv.Engine.Effects;

public static partial class Inject
{
    public static IServiceCollection AddEngineMemStore(this IServiceCollection services)
    {
        return services
            .AddSingleton<IStore<Schema.Engine>, EngineMemStore>()
            .AddSingleton<IEngineStore, EngineMemStore>();
    }
}

public interface IEngineStore : IStore<Schema.Engine>
{
}

public class EngineMemStore : MemStore<Schema.Engine>, IEngineStore
{
}