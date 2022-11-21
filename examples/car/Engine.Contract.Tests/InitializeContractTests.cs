using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestFirst;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests;

public class
    InitializeContractTests : ContractTests<Schema.EngineID, Initialize.IHope, Initialize.IFact, Initialize.Payload>
{
    public InitializeContractTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    protected override void Initialize()
    {
        base.Initialize();
        _newID = TestEnv.ResolveRequired<NewID<Schema.EngineID>>();
    }


    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddModelIDCtor();
    }


    public override void ShouldCreateNewPayload()
    {
        // GIVEN
        var details = Schema.Details.New("Payload Details");
        // WHEN
        var pl = Contract.Initialize.Payload.New(details);
        // THEN
        Assert.NotNull(pl);
    }

    public override void ShouldCreateFactFromBytes()
    {
        // GIVEN
        var aggId = _newID();
        var details = Schema.Details.New("Payload Details");
        var payload = Contract.Initialize.Payload.New(details);
        // WHEN
        var fact = Contract.Initialize.Fact.New(aggId.Id(), payload.ToBytes());
        // THEN
        Assert.NotNull(fact);
    }

    public override void ShouldCreateFactFromPayload()
    {
        // GIVEN
        var aggId = _newID();
        var details = Schema.Details.New("New Engine");
        var payload = Contract.Initialize.Payload.New(details);
        // WHEN
        var fact = Contract.Initialize.Fact.New(aggId.Id(), payload);
        // THEN
        Assert.NotNull(fact);
    }

    public override void ShouldCreateHopeFromBytes()
    {
        // GIVEN
        var aggId = _newID();
        var details = Schema.Details.New("New Engine");
        var payload = Contract.Initialize.Payload.New(details);
        // WHEN
        var hope = Contract.Initialize.Hope.New(aggId.Id(), payload.ToBytes());
        // THEN
        Assert.NotNull(hope);
    }

    public override void ShouldCreateHopeFromPayload()
    {
        // GIVEN
        var aggId = _newID();
        var details = Schema.Details.New("New Engine");
        var payload = Contract.Initialize.Payload.New(details);
        // WHEN
        var fact = Contract.Initialize.Fact.New(aggId.Id(), payload);
        // THEN
        Assert.NotNull(fact);
    }
}