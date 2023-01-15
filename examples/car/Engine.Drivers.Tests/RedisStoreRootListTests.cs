using DotCart.Drivers.Redis;
using DotCart.Drivers.Redis.TestFirst;
using DotCart.TestKit;
using Engine.Context;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Drivers.Tests;

public class RedisStoreRootListTests
    : RedisStoreTestsT<
        IRedisListDbInfo,
        Schema.EngineList,
        Schema.EngineListID>
{
    public RedisStoreRootListTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        base.InjectDependencies(services);
        services
            .AddRootListCtors()
            .AddDotRedis<IRedisListDbInfo, Schema.EngineList, Schema.EngineID>();
    }
}