using DotCart.TestFirst;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Stop;

public class ContractTests : ContractTests<Contract.Schema.EngineID, Contract.Stop.Hope, Contract.Stop.Fact,
    Contract.Stop.Payload>
{
    public ContractTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
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
        // WHEN
        var pl = Contract.Stop.Payload.New();
        // THEN
        Assert.NotNull(pl);
    }

    public override void ShouldCreateNewFact()
    {
        // GIVEN
        var ID = _newID();
        var pl = Contract.Stop.Payload.New();
        // WHEN
        var f = Contract.Stop.Fact.New(ID.Id(), pl);
        // THEN 
        Assert.NotNull(f);
        Assert.NotNull(f.Payload);
    }


    public override void ShouldCreateNewHope()
    {
        // GIVEN
        var ID = _newID();
        var payload = Contract.Stop.Payload.New();
        // WHEN
        var hope = Contract.Stop.Hope.New(ID.Id(), payload);
        // THEN
        Assert.NotNull(hope);
        Assert.IsType<Contract.Stop.Hope>(hope);
    }
}