using DotCart.Drivers.InMem;
using DotCart.Effects;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestEnv.Engine.Effects;

public static class Inject
{
    public static IServiceCollection AddEngineStore(this IServiceCollection services)
    {
        return services
            .AddSingleton<IEngineStore, EngineStore>();
    }
}

public interface IEngineStore : IStore<Schema.Engine>
{
}

public class EngineStore : MemStore<Schema.Engine>, IEngineStore
{
}