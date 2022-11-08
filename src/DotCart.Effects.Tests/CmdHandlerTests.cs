using System.ComponentModel;
using DotCart.Behavior;
using DotCart.Drivers.InMem;
using DotCart.Effects.Drivers;
using DotCart.Schema;
using DotCart.TestEnv.Engine;
using DotCart.TestEnv.Engine.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Effects.Tests;

public class CmdHandlerTests : IoCTests
{
    private IAggregateStoreDriver _aggStoreDriver;
    private ICmdHandler _cmdHandler;
    private NewState<TestEnv.Engine.Schema.Engine> _newEngine;
    private NewSimpleID<SimpleEngineID> _newID;

    public CmdHandlerTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    [Fact]
    public void ShouldResolveCmdHandler()
    {
        var ch = Container.GetService<ICmdHandler>();
        Assert.NotNull(ch);
    }

    [Fact]
    public void ShouldResolveMemEventStore()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var es = Container.GetRequiredService<IAggregateStoreDriver>();
        // THEN
        Assert.NotNull(es);
    }

    [Fact]
    public void ShouldResolveDifferentHandlers()
    {
        // GIVEN
        var ch1 = Container.GetService<ICmdHandler>();
        // WHEN
        var ch2 = Container.GetService<ICmdHandler>();
        // THEN
        Assert.NotSame(ch1, ch2);
    }

    [Fact]
    public async Task ShouldHandleInitializeCmd()
    {
        // GIVEN
        var engineID = _newID();
        var engine = _newEngine();
        var payload = TestEnv.Engine.Initialize.Payload.New(engine);
        var initCmd = TestEnv.Engine.Initialize.Cmd.New(engineID, payload);
        // WHEN
        var fbk = await _cmdHandler.Handle(initCmd);
        // THEN
        if (!fbk.IsSuccess)
        {
            Output.WriteLine(fbk.ToString());
        }
        Assert.True(fbk.IsSuccess);
        
    }

    protected override void Initialize()
    {
        _cmdHandler = Container.GetRequiredService<ICmdHandler>();
        _newEngine = Container.GetRequiredService<NewState<TestEnv.Engine.Schema.Engine>>();
        _aggStoreDriver = Container.GetRequiredService<IAggregateStoreDriver>();
        _newID = Container.GetRequiredService<NewSimpleID<SimpleEngineID>>();
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