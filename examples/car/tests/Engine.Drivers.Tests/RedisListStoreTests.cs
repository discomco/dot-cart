using DotCart.Drivers.Redis;
using DotCart.Drivers.Redis.TestFirst;
using DotCart.TestKit;
using DotCart.TestKit.Mocks;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Drivers.Tests;

public class RedisListStoreTests
    : RedisDocStoreTestsT<
        TheContext.IRedisListDbInfo,
        Schema.EngineList,
        Schema.EngineListID>
{
    public RedisListStoreTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
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
            .AddDotRedis<TheContext.IRedisListDbInfo, Schema.EngineList, Schema.EngineID>();
    }
}