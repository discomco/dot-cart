using DotCart.Context.Abstractions.Drivers;
using DotCart.Contract;
using DotCart.Drivers.InMem;
using DotCart.Drivers.Redis;
using Engine.Contract.Initialize;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Initialize;

public static partial class Inject
{
    public static IServiceCollection AddToDocRedisStoreDriver(this IServiceCollection services)
    {
        return services
            .AddTransient<Actors.IToRedisDoc, Actors.ToRedisDoc>()
            .AddTransient<IModelStore<Common.Schema.Engine>, RedisStore<Common.Schema.Engine>>();
    }
}

public static class Drivers
{
    public class ResponderDriver : MemResponderDriver<Hope>
    {
        public ResponderDriver(GenerateHope<Hope> generateHope) : base(generateHope)
        {
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}