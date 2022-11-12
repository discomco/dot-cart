using DotCart.Context.Effects.Drivers;
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
            .AddTransient<Effects.IToRedisDoc, Effects.ToRedisDoc>()
            .AddTransient<Effects.IToRedisDocStore, Drivers.ToRedisDocStore>()
            .AddTransient<IModelStore<Common.Schema.Engine>, Drivers.ToRedisDocStore>();
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


    public class ToRedisDocStore : RedisStore<Common.Schema.Engine>, Effects.IToRedisDocStore
    {
        public ToRedisDocStore(IRedisDb redisDb) : base(redisDb)
        {
        }
    }
}