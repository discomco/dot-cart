using DotCart.Core;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Engine.Context.Common.Schema;
using Engine.Context.Initialize;
using Engine.Contract.Schema;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize.Drivers.Redis;

public class ToRedisDocDriverTests : RedisStoreDriverTestsT<EngineID, Engine.Context.Common.Schema.Engine>
{
    public ToRedisDocDriverTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
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
            .AddEngineIDCtor()
            .AddEngineCtor()
            .AddToDocRedisStoreDriver();

    }
    
    
    
}