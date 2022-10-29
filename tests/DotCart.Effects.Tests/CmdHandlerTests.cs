using DotCart.Schema;
using DotCart.TestEnv.Engine;
using DotCart.TestEnv.Engine.Schema;
using DotCart.TestKit;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Effects.Tests;

public class CmdHandlerTests : IoCTests
{

    private ICmdHandler _cmdHandler;
    private NewState<Engine> _newEngine;

    [Fact]
    public void ShouldResolveCmdHandler()
    {
        var ch = Container.GetService<ICmdHandler>();
        Assert.NotNull(ch);
    }

    [Fact]
    public void ShouldResolveDifferentHandlers()
    {
        // GIVEN
        var ch1 = Container.GetService<ICmdHandler>();
        // WHEN
        var ch2 = Container.GetService<ICmdHandler>();
        // THEN
        Assert.NotSame(ch1,ch2);
    }

    [Fact]
    public async Task ShouldProcessInitializeCmd()
    {
        // GIVEN
        var engineID = EngineID.New;
        var engine = _newEngine();
        var payload = TestEnv.Engine.Behavior.Initialize.Payload.New(engine);
        var initCmd = TestEnv.Engine.Behavior.Initialize.Cmd.New(engineID, payload);
        // WHEN
        var fbk = await _cmdHandler.Handle(initCmd);
        // THEN
        Assert.True(fbk.IsSuccess);
    }
    
    
   

    public CmdHandlerTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void Initialize()
    {
        _cmdHandler = Container.GetService<ICmdHandler>();
        _newEngine = Container.GetService<NewState<Engine>>();
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddEngineAggregate()
            .AddTransient(_ => A.Fake<IAggregateStore>())
            .AddCmdHandler();
    }
}