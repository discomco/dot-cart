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
    public void ShouldExecuteInitializeCmd()
    {
        // GIVEN
        Assert.NotNull(_agg);
        Assert.NotNull(_engineID);
        _agg.SetID(_engineID);
        // WHEN
        var pl = Schema.Tests.Engine.New();
        var cmd = new Initialize.Cmd(_engineID, pl);
        var feedback = _agg.Execute(_agg.GetState(), cmd);
        var state = feedback.GetPayload<Schema.Tests.Engine>();
        // THEN
        Assert.NotNull(feedback);
        Assert.True(feedback.IsSuccess);
        Assert.True(state.Status.HasFlag(EngineStatus.Initialized));
        Output.WriteLine($"{state}");
    }

    [Fact]
    public void ShouldExecuteStartCmd()
    {
        // GIVEN
        ShouldExecuteInitializeCmd();
        var startCmd = new Start.Cmd(_engineID, new Start.Payload());
        // WHEN
        var feedback = _agg.Execute(_agg.GetState(), startCmd);
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
    }

    protected override void SetTestEnvironment()
    {
        
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddSingleton(Schema.Tests.Engine.New)
            .AddTransient<IEngineAggregate, EngineAggregate>();
    }
}