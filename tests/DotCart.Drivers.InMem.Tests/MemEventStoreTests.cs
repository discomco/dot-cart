using DotCart.Effects;
using DotCart.TestEnv.Engine;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.InMem.Tests;

public class MemEventStoreTests : IoCTests
{

    private IEventStore? _eventStore;
    
    
    [Fact]
    public void ShouldResolveMemEventStore()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var me = Container.GetService<IEventStore>();
        // THEN
        Assert.NotNull(me);
    }


    [Fact]
    public void ShouldSaveAggregate()
    {
        // GIVEN
        Assert.NotNull(_eventStore);
        // AND
        
        
    }

    public MemEventStoreTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void Initialize()
    {
        _eventStore = Container.GetRequiredService<IEventStore>();
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