using DotCart.Abstractions.Drivers;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.Redis.Tests;

internal class MyRedisStore : RedisStore<TheSchema.Doc>
{
    public MyRedisStore(IRedisDbT<TheSchema.Doc> redisDb) : base(redisDb)
    {
    }
}

internal static class Inject
{
    public static IServiceCollection AddMyRedisStoreDriver(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => TheSchema.Doc.Rand)
            .AddTransient(_ => TheSchema.ID.Ctor)
            .AddTransient<IDocStore<TheSchema.Doc>, MyRedisStore>();
    }
}

public class MyRedisStoreDriverTests : RedisStoreDriverTestsT<TheSchema.ID, TheSchema.Doc>
{
    public MyRedisStoreDriverTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddMyRedisStoreDriver()
            .AddSingletonRedisDb<TheSchema.Doc>();
    }
}