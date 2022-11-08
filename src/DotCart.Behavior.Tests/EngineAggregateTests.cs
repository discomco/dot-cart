using DotCart.Schema;
using DotCart.TestEnv.Engine;
using DotCart.TestEnv.Engine.Schema;
using DotCart.TestFirst;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;


namespace DotCart.Behavior.Tests;

public class EngineAggregateTests : AggregateTests<EngineID, Engine>
{
    private IDomainPolicy? _startPolicy;

    public EngineAggregateTests(ITestOutputHelper output, IoCTestContainer container)
        : base(output, container)
    {
    }


    [Fact]
    public void ShouldLoadEvents()
    {
        // GIVEN
        Assert.NotNull(_builder);
        _agg = _builder.Build();
        Assert.NotNull(_agg);
        // AND
        var events = ScenariosAndStreams.InitializeEngineWithThrottleUpEventStream(_ID, _newState);
        _agg.SetID(_ID);
        _agg.Load(events);
        // THEN
        Assert.Equal(2, _agg.Version);
    }


    [Fact]
    public async Task ShouldExecuteInitializeCmd()
    {
        // GIVEN
        Assert.NotNull(_builder);
        _agg = _builder.Build();
        Assert.NotNull(_agg);
        Assert.NotNull(_ID);
        _agg.SetID(_ID);
        // WHEN
        var eng = _newState();
        eng.Id = _ID.Id();
        var cmd = TestEnv.Engine.Initialize.Cmd.New(_ID,
            TestEnv.Engine.Initialize.Payload.New(eng));
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
        var startCmd = Start.Cmd.New(_ID, Start.Payload.New);
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

    [Fact]
    public void ShouldResolveStartTryCmd()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var builder = Container.GetRequiredService<IAggregateBuilder>();
        var agg = builder.Build();
        var topic = Topic.Get<Start.Cmd>();
        var knowsIt = agg.KnowsTry(topic);
        // THEN
        Assert.True(knowsIt);
    }

    // protected override void Initialize()
    // { 
    //     base.Initialize();
    //     // _builder = Container.GetService<IAggregateBuilder>();
    //     // _newState = Container.GetService<NewState<Engine>>();
    //     // _agg = _builder.Build();
    //     // _newID = Container.GetRequiredService<NewID<EngineID>>();
    // }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {

        services
            .AddEngineIDCtor()
            .AddEngineAggregate()
            .AddAggregateBuilder()
            .AddStartBehavior()
            .AddInitializeBehavior()
            .AddThrottleUpBehavior();
    }
}