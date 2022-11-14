using DotCart.Context.Abstractions.Drivers;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.Redis.Tests;

internal class MyRedisStore : RedisStore<TheDoc>
{
    public MyRedisStore(
        IRedisDb redisDb) : base(redisDb)
    {
    }
}

internal static class Inject
{
    public static IServiceCollection AddMyRedisStoreDriver(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => TheDoc.Rand)
            .AddTransient(_ => MyID.Ctor)
            .AddTransient<IModelStore<TheDoc>, MyRedisStore>();
    }
}

public class MyRedisStoreDriverTests : RedisStoreDriverTestsT<MyID, TheDoc>
{
    public MyRedisStoreDriverTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddMyRedisStoreDriver()
            .AddSingletonRedisDb<TheDoc>();
    }
}