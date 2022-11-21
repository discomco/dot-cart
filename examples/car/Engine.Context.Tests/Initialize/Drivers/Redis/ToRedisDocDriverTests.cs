using DotCart.Core;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Engine.Context.Common.Schema;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize.Drivers.Redis;

public class ToRedisDocDriverTests : RedisStoreDriverTestsT<Schema.EngineID, Common.Schema.Engine>
{
    public ToRedisDocDriverTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetTestEnvironment()
    {
        DotEnv.FromEmbedded();
    }


    protected override void InjectDependencies(IServiceCollection services)
    {
        base.InjectDependencies(services);
        services
            .AddModelIDCtor()
            .AddModelCtor()
            .AddRedisStoreDriver();
    }
}