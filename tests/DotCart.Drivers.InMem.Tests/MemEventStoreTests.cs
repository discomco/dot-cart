using DotCart.Effects;
using DotCart.TestEnv.Engine;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.InMem.Tests;

public class MemEventStoreTests : IoCTests
{
    private IAggregateStore? _aggStore;

    public MemEventStoreTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }


    [Fact]
    public void ShouldResolveMemEventStore()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var me = Container.GetService<IAggregateStore>();
        // THEN
        Assert.NotNull(me);
    }


    [Fact]
    public void ShouldSaveAggregate()
    {
        // GIVEN
        Assert.NotNull(_aggStore);
        // AND
    }

    protected override void Initialize()
    {
        _aggStore = Container.GetRequiredService<IAggregateStore>();
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddEngineAggregate()
            .AddMemEventStore();
    }
}