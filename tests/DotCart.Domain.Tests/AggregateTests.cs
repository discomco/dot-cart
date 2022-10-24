using DotCart.Domain.Tests.Engine;
using DotCart.Schema;
using DotCart.Schema.Tests;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Domain.Tests;

public class AggregateTests : IoCTests
{

    private EngineID _engineID = EngineID.New;
    private IEngineAggregate _agg;
    private IEnginePolicy _startPolicy;
    private IEngineAggregateBuilder _builder;
    private NewState<Schema.Tests.Engine> _newState; 

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
    public async Task ShouldExecuteInitializeCmd()
    {
        // GIVEN
        Assert.NotNull(_agg);
        Assert.NotNull(_engineID);
        _agg.SetID(_engineID);
        // WHEN
        var eng = _newState();
        eng.Id = _engineID.Value;
        var cmd = Engine.Initialize.Cmd.New(_engineID, Engine.Initialize.Payload.New(eng));
        var feedback = await _agg.ExecuteAsync(cmd);
        var state = feedback.GetPayload<Schema.Tests.Engine>();
        // THEN
        Assert.NotNull(feedback);
        
        Assert.True(feedback.IsSuccess);
        Assert.True(state.Status.HasFlag(EngineStatus.Initialized));
//        Thread.Sleep(1_000);
        state = (Schema.Tests.Engine)_agg.GetState();
        Output.WriteLine($"{state}");
    }

    [Fact]
    public async Task ShouldExecuteStartCmd()
    {
        // GIVEN
        await ShouldExecuteInitializeCmd();
        var startCmd = Engine.Start.Cmd.New(_engineID, Engine.Start.Payload.New);
        // WHEN
        var feedback = await _agg.ExecuteAsync(startCmd);
        var state = feedback.GetPayload<Schema.Tests.Engine>();
        // THEN
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
        _builder = Container.GetService<IEngineAggregateBuilder>();
        _agg = _builder.Aggregate;
        _newState = Container.GetService<NewState<Schema.Tests.Engine>>();
    }

    protected override void SetTestEnvironment()
    {
        
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddAggregateBuilder();

    }
}