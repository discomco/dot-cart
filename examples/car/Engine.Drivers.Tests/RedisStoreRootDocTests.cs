using DotCart.Drivers.Redis;
using DotCart.Drivers.Redis.TestFirst;
using DotCart.TestKit;
using Engine.Context;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Drivers.Tests;

public class RedisStoreRootDocTests
    : RedisStoreTestsT<IRedisDocDbInfo, Schema.Engine, Schema.EngineID>
{
    public RedisStoreRootDocTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddRootDocCtors()
            .AddDotRedis<IRedisDocDbInfo, Schema.Engine, Schema.EngineID>();
    }
}