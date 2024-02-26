using DotCart.Drivers.Redis;
using DotCart.Drivers.Redis.TestFirst;
using DotCart.TestKit;
using DotCart.TestKit.Mocks;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Drivers.Tests;

public class RedisDocStoreTests
    : RedisDocStoreTestsT<TheContext.IRedisDocDbInfo, Schema.Engine, Schema.EngineID>
{
    public RedisDocStoreTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }


    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddRootDocCtors()
            .AddDotRedis<TheContext.IRedisDocDbInfo, Schema.Engine, Schema.EngineID>();
    }
}