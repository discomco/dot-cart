using DotCart.Defaults;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Engine.Context;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Drivers.Tests;

public class RedisStoreRootDocDriverTests
    : RedisStoreDriverTestsT<Schema.EngineID, Schema.Engine>
{
    public RedisStoreRootDocDriverTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddRootDocCtors()
            .AddDefaultDrivers<IEngineProjectorInfo, Schema.Engine, Schema.EngineList>();
    }
}