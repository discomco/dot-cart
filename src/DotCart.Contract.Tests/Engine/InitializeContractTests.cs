using System.ComponentModel;
using DotCart.Schema;
using DotCart.TestEnv.Engine;
using DotCart.TestEnv.Engine.Schema;
using DotCart.TestFirst;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Contract.Tests.Engine;

public class InitializeContractTests: ContractTests<EngineID,Initialize.Hope, Initialize.Fact, Initialize.Payload>
{

    protected NewState<TestEnv.Engine.Schema.Engine> NewEngine;

    protected override void Initialize()
    {
        base.Initialize();
        NewEngine = Container.GetRequiredService<NewState<TestEnv.Engine.Schema.Engine>>();
    }
    
    public InitializeContractTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }


    protected override void SetTestEnvironment()
    {
        
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddStateCtor();
    }


    public override void ShouldCreateNewPayload()
    {
        // GIVEN
        var engine = TestEnv.Engine.Schema.Engine.New(
            EngineID.New.Value, 
            EngineStatus.Initialized,
            Details.New("Payload Details"));
        // WHEN
        var pl = TestEnv.Engine.Initialize.Payload.New(engine);
        // THEN
        Assert.NotNull(pl);
    }

    public override void ShouldCreateFactFromBytes()
    {
        // GIVEN
        var aggId = EngineID.New;
        var engine = NewEngine();
        var payload = TestEnv.Engine.Initialize.Payload.New(engine);
        // WHEN
        var fact = TestEnv.Engine.Initialize.Fact.New(aggId.Value, payload.ToBytes());
        // THEN
        Assert.NotNull(fact);
    }

    public override void ShouldCreateFactFromPayload()
    {
        // GIVEN
        var aggId = EngineID.New;
        var engine = NewEngine();
        var payload = TestEnv.Engine.Initialize.Payload.New(engine);
        // WHEN
        var fact = TestEnv.Engine.Initialize.Fact.New(aggId.Value, payload);
        // THEN
        Assert.NotNull(fact);
    }

    public override void ShouldCreateHopeFromBytes()
    {
        // GIVEN
        var aggId = EngineID.New;
        var engine = NewEngine();
        var payload = TestEnv.Engine.Initialize.Payload.New(engine);
        // WHEN
        var fact = TestEnv.Engine.Initialize.Hope.New(aggId.Value, payload.ToBytes());
        // THEN
        Assert.NotNull(fact);

    }

    public override void ShouldCreateHopeFromPayload()
    {
        // GIVEN
        var aggId = EngineID.New;
        var engine = NewEngine();
        var payload = TestEnv.Engine.Initialize.Payload.New(engine);
        // WHEN
        var fact = TestEnv.Engine.Initialize.Fact.New(aggId.Value, payload);
        // THEN
        Assert.NotNull(fact);
    }
}