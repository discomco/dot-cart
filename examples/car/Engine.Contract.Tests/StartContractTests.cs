using DotCart.Core;
using DotCart.TestFirst;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests;

public class StartContractTests : ContractTests<Schema.EngineID, Start.IHope, Start.IFact, Start.Payload>
{
    public StartContractTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    public override void ShouldCreateNewPayload()
    {
        // GIVEN
        // WHEN
        var pl = Start.Payload.New;
        // THEN
        Assert.NotNull(pl);
    }

    public override void ShouldCreateFactFromBytes()
    {
        // GIVEN
        var pl = Start.Payload.New;
        var ID = _newID();
        // WHEN
        var fact = Start.Fact.New(ID.Id(), pl.ToBytes());
        // THEN
        Assert.NotNull(fact);
    }

    public override void ShouldCreateFactFromPayload()
    {
        var pl = Start.Payload.New;
        var ID = _newID();
        // WHEN
        var fact = Start.Fact.New(ID.Id(), pl);
        // THEN
        Assert.NotNull(fact);
    }

    public override void ShouldCreateHopeFromBytes()
    {
        var pl = Start.Payload.New;
        var ID = _newID();
        // WHEN
        var hope = Start.Hope.New(ID.Id(), pl.ToBytes());
        // THEN
        Assert.NotNull(hope);
    }

    public override void ShouldCreateHopeFromPayload()
    {
        var pl = Start.Payload.New;
        var ID = _newID();
        // WHEN
        var hope = Start.Hope.New(ID.Id(), pl);
        // THEN
        Assert.NotNull(hope);
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddModelIDCtor();
    }
}