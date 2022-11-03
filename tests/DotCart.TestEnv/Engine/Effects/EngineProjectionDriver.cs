using DotCart.Drivers;
using DotCart.Effects;
using DotCart.Effects.Drivers;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestEnv.Engine.Effects;

public static partial class Inject
{
    public static IServiceCollection AddEngineMemProjectionDriver(this IServiceCollection services)
    {
        return services
            .AddEngineMemStore()
            .AddSingleton<IEngineMemProjectionDriver, EngineProjectionDriver>()
            .AddSingleton<IProjectionDriver<Schema.Engine>, EngineProjectionDriver>();
    }
}

public class EngineProjectionDriver : ProjectionDriver<Schema.Engine>, IEngineMemProjectionDriver
{
    public EngineProjectionDriver(IStore<Schema.Engine> store) : base(store)
    {
    }
}

public interface IEngineMemProjectionDriver : IProjectionDriver<Schema.Engine>
{
}