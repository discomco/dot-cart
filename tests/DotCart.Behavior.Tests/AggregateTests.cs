using DotCart.Schema;
using DotCart.TestEnv.Engine;
using DotCart.TestEnv.Engine.Behavior;
using DotCart.TestEnv.Engine.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Behavior.Tests;

public class AggregateTests : IoCTests
{
    private readonly EngineID? _engineID = EngineID.New;
    private IAggregate? _agg;
    private IAggregateBuilder? _builder;
    private NewState<Engine>? _newState;
    private IDomainPolicy? _startPolicy;

    public AggregateTests(ITestOutputHelper output, IoCTestContainer container)
        : base(output, container)
    {
    }

    [Fact]
    public void ShouldBeAbleToCreateEngineID()
    {
        // GIVEN
        // WHEN
        var ID = EngineID.New;
        // THEN
        Assert.NotNull(ID);
    }

    [Fact]
    public void ShouldResolveBehavior()
    {
        // GIVEN
        // WHEN
        var agg = Container.GetService<IAggregate>();
        // THEN
        Assert.NotNull(agg);
        var state = agg.GetState();
        Assert.NotNull(state);
    }

    [Fact]
    public void ShouldLoadEvents()
    {
        // GIVEN
        Assert.NotNull(_agg);
        // AND
        var events = HelperFuncs.CreateEngineEvents(_engineID, _newState);
        _agg.SetID(_engineID);
        _agg.Load(events);
        // THEN
        Assert.Equal(2, _agg.Version);
    }


    [Fact]
    public async Task ShouldExecuteInitializeCmd()
    {
        // GIVEN
        Assert.NotNull(_agg);
        Assert.NotNull(_engineID);
        _agg.SetID(_engineID);
        // WHEN
        var eng = _newState();
        eng.Id = _engineID.Value;
        var cmd = TestEnv.Engine.Behavior.Initialize.Cmd.New(_engineID,
            TestEnv.Engine.Behavior.Initialize.Payload.New(eng));
        var feedback = await _agg.ExecuteAsync(cmd);
        var state = feedback.GetPayload<Engine>();
        // THEN
        Assert.NotNull(feedback);
        if (!feedback.IsSuccess) Output.WriteLine(feedback.ErrState.ToString());

        Assert.True(feedback.IsSuccess);
        Assert.True(state.Status.HasFlag(EngineStatus.Initialized));
//        Thread.Sleep(1_000);
        state = (Engine)_agg.GetState();
        Output.WriteLine($"{state}");
    }

    [Fact]
    public async Task ShouldExecuteStartCmd()
    {
        // GIVEN
        await ShouldExecuteInitializeCmd();
        var startCmd = Start.Cmd.New(_engineID, Start.Payload.New);
        // WHEN
        var feedback = await _agg.ExecuteAsync(startCmd);
        var state = feedback.GetPayload<Engine>();
        // THEN
        if (!feedback.IsSuccess) Output.WriteLine(feedback.ErrState.ToString());
        Assert.NotNull(feedback);
        Assert.True(feedback.IsSuccess);
        Assert.True(((int)state.Status).HasAllFlags(
            (int)EngineStatus.Initialized,
            (int)EngineStatus.Started));
        Output.WriteLine($"{state}");
    }

    protected override void Initialize()
    {
        _builder = Container.GetService<IAggregateBuilder>();
        _newState = Container.GetService<NewState<Engine>>();
        _agg = _builder.Build();
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        var s = services
            .AddEngineAggregate()
            .AddAggregateBuilder();
    }
}