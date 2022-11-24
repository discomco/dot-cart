using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.Core;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Contract;
using Engine.Utils;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests;

public class AggregateTests : AggregateTestsT<Schema.EngineID, Engine>
{
    private IAggregatePolicy? _startPolicy;

    public AggregateTests(ITestOutputHelper output, IoCTestContainer testEnv)
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
        var events = ScenariosAndStreams.InitializeWithThrottleUpEvents(_ID);
        _agg.SetID(_ID);
        _agg.Load(events);
        // THEN
        Assert.Equal(events.Count() - 1, _agg.Version);
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
        var details = Schema.Details.New("New Engine");
        var cmd = Behavior.Initialize.Cmd.New(_ID, Contract.Initialize.Payload.New(details));
        var feedback = await _agg.ExecuteAsync(cmd);
        var state = feedback.GetPayload<Engine>();
        // THEN
        Assert.NotNull(feedback);
        if (!feedback.IsSuccess) Output.WriteLine(feedback.ErrState.ToString());

        Assert.True(feedback.IsSuccess);
        var isInitialized = state.Status.HasFlag(Schema.EngineStatus.Initialized);
        Assert.True(isInitialized);
//        Thread.Sleep(1_000);
        state = (Engine)_agg.GetState();
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
        var state = feedback.GetPayload<Engine>();
        // THEN
        if (!feedback.IsSuccess) Output.WriteLine(feedback.ErrState.ToString());
        Assert.NotNull(feedback);
        Assert.True(feedback.IsSuccess);
        Assert.True(((int)state.Status).HasAllFlags(
            (int)Schema.EngineStatus.Initialized,
            (int)Schema.EngineStatus.Started));
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
            .AddEngineBehavior();
    }
}