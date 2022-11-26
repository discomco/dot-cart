using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.TestKit;
using Engine.Contract;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests;

public class CmdHandlerTests : IoCTests
{
    private IAggregateStore _aggStore;
    private ICmdHandler _cmdHandler;
    private StateCtor<Engine> _newEngine;
    private IDCtor<Schema.EngineID> _newID;

    public CmdHandlerTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveCmdHandler()
    {
        var ch = TestEnv.Resolve<ICmdHandler>();
        Assert.NotNull(ch);
    }

    [Fact]
    public void ShouldResolveMemEventStore()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var es = TestEnv.ResolveRequired<IAggregateStore>();
        // THEN
        Assert.NotNull(es);
    }

    [Fact]
    public void ShouldResolveDifferentHandlers()
    {
        // GIVEN
        var ch1 = TestEnv.Resolve<ICmdHandler>();
        // WHEN
        var ch2 = TestEnv.Resolve<ICmdHandler>();
        // THEN
        Assert.NotSame(ch1, ch2);
    }

    [Fact]
    public async Task ShouldHandleInitializeCmd()
    {
        // GIVEN
        var engineID = _newID();
        var details = Schema.Details.New("New engine");
        var payload = Contract.Initialize.Payload.New(details);
        var initCmd = Behavior.Initialize.Cmd.New(engineID, payload);
        // WHEN
        var fbk = await _cmdHandler.HandleAsync(initCmd);
        // THEN
        if (!fbk.IsSuccess) Output.WriteLine(fbk.ToString());
        Assert.True(fbk.IsSuccess);
    }

    protected override void Initialize()
    {
        _cmdHandler = TestEnv.ResolveRequired<ICmdHandler>();
        _newEngine = TestEnv.ResolveRequired<StateCtor<Engine>>();
        _aggStore = TestEnv.ResolveRequired<IAggregateStore>();
        _newID = TestEnv.ResolveRequired<IDCtor<Schema.EngineID>>();
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddTransient(_ => A.Fake<IAggregateStore>())
            .AddEngineBehavior()
            .AddCmdHandler();
    }
}