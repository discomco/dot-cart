using DotCart.Drivers.Redis.TestFirst;
using DotCart.TestKit;
using DotCart.TestKit.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.Redis.Tests;

public class TheRedisDocStoreTests
    : RedisDocStoreTestsT<
        TheContext.IRedisDocDbInfo,
        TheSchema.Doc,
        TheSchema.DocID>
{
    public TheRedisDocStoreTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddTheDocCtors()
            .AddDotRedis<TheContext.IRedisDocDbInfo, TheSchema.Doc, TheSchema.DocID>();
    }
}