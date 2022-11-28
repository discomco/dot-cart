using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestFirst;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Initialize;

public class
    InitializeContractTests : ContractTests<Contract.Schema.EngineID, Contract.Initialize.IHope,
        Contract.Initialize.IFact, Contract.Initialize.Payload>
{
    public InitializeContractTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    protected override void Initialize()
    {
        base.Initialize();
        _newID = TestEnv.ResolveRequired<IDCtorT<Contract.Schema.EngineID>>();
    }


    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddIDCtor();
    }


    public override void ShouldCreateNewPayload()
    {
        // GIVEN
        var details = Contract.Schema.Details.New("Payload Details");
        // WHEN
        var pl = Contract.Initialize.Payload.New(details);
        // THEN
        Assert.NotNull(pl);
    }

    public override void ShouldCreateNewFact()
    {
        // GIVEN
        var aggId = _newID();
        var details = Contract.Schema.Details.New("Payload Details");
        var payload = Contract.Initialize.Payload.New(details);
        // WHEN
        var fact = Contract.Initialize.Fact.New(aggId.Id(), payload.ToBytes());
        // THEN
        Assert.NotNull(fact);
    }

    public override void ShouldCreateNewHope()
    {
        // GIVEN
        var aggId = _newID();
        var details = Contract.Schema.Details.New("New Engine");
        var payload = Contract.Initialize.Payload.New(details);
        // WHEN
        var hope = Contract.Initialize.Hope.New(aggId.Id(), payload.ToBytes());
        // THEN
        Assert.NotNull(hope);
    }
}