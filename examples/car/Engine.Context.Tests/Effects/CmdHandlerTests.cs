using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Contract.Schemas;
using DotCart.Drivers.InMem;
using DotCart.TestKit;
using Engine.Context.Common;
using Engine.Context.Initialize;
using Engine.Contract.Initialize;
using Engine.Contract.Schema;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Effects;

public class CmdHandlerTests : IoCTests
{
    private IAggregateStoreDriver _aggStoreDriver;
    private ICmdHandler _cmdHandler;
    private NewState<Common.Schema.Engine> _newEngine;
    private NewID<EngineID> _newID;

    public CmdHandlerTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveCmdHandler()
    {
        var ch = TestEnv.GetService<ICmdHandler>();
        Assert.NotNull(ch);
    }

    [Fact]
    public void ShouldResolveMemEventStore()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var es = TestEnv.GetRequiredService<IAggregateStoreDriver>();
        // THEN
        Assert.NotNull(es);
    }

    [Fact]
    public void ShouldResolveDifferentHandlers()
    {
        // GIVEN
        var ch1 = TestEnv.GetService<ICmdHandler>();
        // WHEN
        var ch2 = TestEnv.GetService<ICmdHandler>();
        // THEN
        Assert.NotSame(ch1, ch2);
    }

    [Fact]
    public async Task ShouldHandleInitializeCmd()
    {
        // GIVEN
        var engineID = _newID();
        var details = Details.New("New engine");
        var payload = Payload.New(details);
        var initCmd = Cmd.New(engineID, payload);
        // WHEN
        var fbk = await _cmdHandler.HandleAsync(initCmd);
        // THEN
        if (!fbk.IsSuccess) Output.WriteLine(fbk.ToString());
        Assert.True(fbk.IsSuccess);
    }

    protected override void Initialize()
    {
        _cmdHandler = TestEnv.GetRequiredService<ICmdHandler>();
        _newEngine = TestEnv.GetRequiredService<NewState<Common.Schema.Engine>>();
        _aggStoreDriver = TestEnv.GetRequiredService<IAggregateStoreDriver>();
        _newID = TestEnv.GetRequiredService<NewID<EngineID>>();
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddInitializeBehavior()
            .AddEngineAggregate()
            .AddAggregateBuilder()
            .AddMemEventStore()
            .AddCmdHandler();
    }
}