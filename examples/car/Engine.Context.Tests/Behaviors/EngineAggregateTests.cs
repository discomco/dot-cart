using DotCart.Context.Abstractions;
using DotCart.Context.Behaviors;
using DotCart.Core;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Context.ChangeRpm;
using Engine.Context.Common;
using Engine.Context.Initialize;
using Engine.Context.Start;
using Engine.Contract.Initialize;
using Engine.Contract.Schema;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Cmd = Engine.Context.Initialize.Cmd;

namespace Engine.Context.Tests.Behaviors;

public class EngineAggregateTests : AggregateTestsT<EngineID, Common.Schema.Engine>
{
    private IAggregatePolicy? _startPolicy;

    public EngineAggregateTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
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
        var events = ScenariosAndStreams.InitializeEngineWithThrottleUpEventStream(_ID);
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
        var details = Details.New("New Engine");
        var cmd = Cmd.New(_ID, Payload.New(details));
        var feedback = await _agg.ExecuteAsync(cmd);
        var state = feedback.GetPayload<Common.Schema.Engine>();
        // THEN
        Assert.NotNull(feedback);
        if (!feedback.IsSuccess) Output.WriteLine(feedback.ErrState.ToString());

        Assert.True(feedback.IsSuccess);
        Assert.True(state.Status.HasFlag(EngineStatus.Initialized));
//        Thread.Sleep(1_000);
        state = (Common.Schema.Engine)_agg.GetState();
        Output.WriteLine($"{state}");
    }

    [Fact]
    public async Task ShouldExecuteStartCmd()
    {
        // GIVEN
        await ShouldExecuteInitializeCmd();
        var startCmd = Start.Cmd.New(_ID, Contract.Start.Payload.New);
        // WHEN
        var feedback = await _agg.ExecuteAsync(startCmd);
        var state = feedback.GetPayload<Common.Schema.Engine>();
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
        Assert.NotNull(TestEnv);
        // WHEN
        var builder = TestEnv.ResolveRequired<IAggregateBuilder>();
        var agg = builder.Build();
        var topic = TopicAtt.Get<Start.Cmd>();
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
            .AddModelIDCtor()
            .AddEngineAggregate()
            .AddAggregateBuilder()
            .AddInitializeBehavior()
            .AddStartBehavior()
            .AddInitializeBehavior()
            .AddChangeRpmBehavior();
    }
}