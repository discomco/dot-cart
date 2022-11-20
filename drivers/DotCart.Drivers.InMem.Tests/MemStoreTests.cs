using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Context.Effects;
using DotCart.TestKit;
using Engine.Context.Common;
using Engine.Contract.Schema;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.InMem.Tests;

public class MemStoreTests : IoCTests
{
    private IModelStore<Engine.Context.Common.Schema.Engine> _engModelStore;
    private NewState<Engine.Context.Common.Schema.Engine> _newEngine;


    public MemStoreTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveMemStore()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var engStore = TestEnv.ResolveRequired<IModelStore<Engine.Context.Common.Schema.Engine>>();
        // THEN
        Assert.NotNull(engStore);
    }

    [Fact]
    public void ShouldAddEnginesToStore()
    {
        // GIVEN
        Assert.NotNull(_engModelStore);
        Assert.NotNull(_newEngine);
        var eng1 = _newEngine();
        eng1.Id = EngineID.New().Id();
        eng1.Details = Details.New("Engine1", "The First Engine");

        var eng2 = _newEngine();
        eng2.Id = EngineID.New().Id();
        eng2.Details = Details.New("Engine2", "The Second Engine");

        // WHEN
        Task.WaitAll(
            _engModelStore.SetAsync(eng1.Id, eng1),
            _engModelStore.SetAsync(eng2.Id, eng2)
        );
        //  THEN
        Assert.True(_engModelStore.Exists(eng1.Id).Result);
        Assert.True(_engModelStore.Exists(eng2.Id).Result);
    }

    protected override void Initialize()
    {
        _engModelStore = TestEnv.ResolveRequired<IModelStore<Engine.Context.Common.Schema.Engine>>();
        _newEngine = TestEnv.ResolveRequired<NewState<Engine.Context.Common.Schema.Engine>>();
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddEngineAggregate()
            .AddCmdHandler()
            .AddSingleton<IModelStore<Engine.Context.Common.Schema.Engine>,
                MemStore<Engine.Context.Common.Schema.Engine>>();
    }
}