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
    private IEngineAggregate? _agg;
    private IEnginePolicy? _startPolicy;
    private IAggregateBuilder? _builder;
    private NewState<Engine>? _newState; 

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
    public void ShouldBeAbleToCreateAnAggregate()
    {
        // GIVEN
        // WHEN
        var agg =Container.GetService<IEngineAggregate>();
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
        var initPayload = TestEnv.Engine.Behavior.Initialize.Payload.New(_newState());
        var initEvt = TestEnv.Engine.Behavior.Initialize.Evt.New(_engineID, initPayload);
        // AND
        var startPayload = Start.Payload.New;
        var startEvt = Start.Evt.New(_engineID, startPayload);
        // AND
        var throttleUpPayload = ThrottleUp.Payload.New(5);
        var throttleUpEvt = ThrottleUp.Evt.New(_engineID, throttleUpPayload);
//        var throttleUpCmd = ThrottleUp.Cmd.New(_engineID, throttleUpPayload);
        // WHEN
        var events = new IEvt[] { initEvt, startEvt, throttleUpEvt };
        _agg.Load(events);
        // THEN
        Assert.Equal(2,_agg.Version);
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
        var cmd = TestEnv.Engine.Behavior.Initialize.Cmd.New(_engineID, TestEnv.Engine.Behavior.Initialize.Payload.New(eng));
        var feedback = await _agg.ExecuteAsync(cmd);
        var state = feedback.GetPayload<Engine>();
        // THEN
        Assert.NotNull(feedback);
        if (!feedback.IsSuccess)
        {
            Output.WriteLine(feedback.ErrState.ToString());
        }
        
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
        if (!feedback.IsSuccess)
        {
            Output.WriteLine(feedback.ErrState.ToString());
            
        }
        Assert.NotNull(feedback);
        Assert.True(feedback.IsSuccess);
        Assert.True(((int)state.Status).HasAllFlags(
            (int)EngineStatus.Initialized, 
            (int)EngineStatus.Started));
        Output.WriteLine($"{state}");
    }

    public AggregateTests(ITestOutputHelper output, IoCTestContainer container) 
        : base(output, container)
    {
    }

    protected override void Initialize()
    {
        _agg = Container.GetService<IEngineAggregate>();
        _builder = Container.GetService<IAggregateBuilder>();
        _newState = Container.GetService<NewState<Engine>>();
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