using DotCart.Effects;
using DotCart.Effects.Drivers;
using DotCart.Schema;
using DotCart.TestEnv.Engine;
using DotCart.TestEnv.Engine.Effects;
using DotCart.TestEnv.Engine.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.InMem.Tests;

public class MemStoreTests : IoCTests
{
    private IModelStoreDriver<Engine> _engModelStoreDriver;
    private NewState<Engine> _newEngine;


    public MemStoreTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    [Fact]
    public void ShouldResolveMemStore()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var engStore = Container.GetRequiredService<IEngineModelStoreDriver>();
        // THEN
        Assert.NotNull(engStore);
    }

    [Fact]
    public void ShouldAddEnginesToStore()
    {
        // GIVEN
        Assert.NotNull(_engModelStoreDriver);
        Assert.NotNull(_newEngine);
        var eng1 = _newEngine();
        eng1.Details = Details.New("Engine1", "The First Engine");
        var eng2 = _newEngine();
        eng2.Details = Details.New("Engine2", "The Second Engine");
        // WHEN
        Task.WaitAll(
            _engModelStoreDriver.SetAsync(eng1.Id, eng1),
            _engModelStoreDriver.SetAsync(eng2.Id, eng2)
        );
        //  THEN
        Assert.True(_engModelStoreDriver.Exists(eng1.Id).Result);
        Assert.True(_engModelStoreDriver.Exists(eng2.Id).Result);
    }

    protected override void Initialize()
    {
        _engModelStoreDriver = Container.GetRequiredService<IEngineModelStoreDriver>();
        _newEngine = Container.GetRequiredService<NewState<Engine>>();
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddEngineAggregate()
            .AddCmdHandler()
            .AddEngineMemStore();
    }
}