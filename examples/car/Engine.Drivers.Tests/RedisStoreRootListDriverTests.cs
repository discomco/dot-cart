using DotCart.Defaults;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Engine.Context;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Drivers.Tests;

public class RedisStoreRootListDriverTests
    : RedisStoreDriverTestsT<Schema.EngineListID, Schema.EngineList>

{
    public RedisStoreRootListDriverTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddRootListCtors()
            .AddDefaultDrivers<IEngineProjectorInfo, Schema.Engine, Schema.EngineList>();
    }
}