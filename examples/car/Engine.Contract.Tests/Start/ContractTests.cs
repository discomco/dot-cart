using DotCart.Core;
using DotCart.TestFirst;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Start;

public class ContractTests : ContractTests<Contract.Schema.EngineID, Contract.Start.IHope, Contract.Start.IFact,
    Contract.Start.Payload>
{
    public ContractTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    public override void ShouldCreateNewPayload()
    {
        // GIVEN
        // WHEN
        var pl = Contract.Start.Payload.New;
        // THEN
        Assert.NotNull(pl);
    }

    public override void ShouldCreateNewFact()
    {
        // GIVEN
        var pl = Contract.Start.Payload.New;
        var ID = _newID();
        // WHEN
        var fact = Contract.Start.Fact.New(ID.Id(), pl.ToBytes());
        // THEN
        Assert.NotNull(fact);
    }


    public override void ShouldCreateNewHope()
    {
        var pl = Contract.Start.Payload.New;
        var ID = _newID();
        // WHEN
        var hope = Contract.Start.Hope.New(ID.Id(), pl.ToBytes());
        // THEN
        Assert.NotNull(hope);
    }


    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddIDCtor();
    }
}