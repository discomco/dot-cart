using DotCart.Core;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Client.Schema;
using Engine.Client.Start;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Payload = Engine.Client.Start.Payload;

namespace Engine.Client.Tests;

public class StartContractTests : ContractTests<EngineID, IHope, IFact, Payload>
{
    public StartContractTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }


    public override void ShouldCreateNewPayload()
    {
        // GIVEN
        // WHEN
        var pl = Payload.New;
        // THEN
        Assert.NotNull(pl);
    }

    public override void ShouldCreateFactFromBytes()
    {
        // GIVEN
        var pl = Payload.New;
        var ID = _newID();
        // WHEN
        var fact = Fact.New(ID.Id(), pl.ToBytes());
        // THEN
        Assert.NotNull(fact);
    }

    public override void ShouldCreateFactFromPayload()
    {
        var pl = Payload.New;
        var ID = _newID();
        // WHEN
        var fact = Fact.New(ID.Id(), pl);
        // THEN
        Assert.NotNull(fact);
    }

    public override void ShouldCreateHopeFromBytes()
    {
        var pl = Payload.New;
        var ID = _newID();
        // WHEN
        var hope = Hope.New(ID.Id(), pl.ToBytes());
        // THEN
        Assert.NotNull(hope);
    }

    public override void ShouldCreateHopeFromPayload()
    {
        var pl = Payload.New;
        var ID = _newID();
        // WHEN
        var hope = Hope.New(ID.Id(), pl);
        // THEN
        Assert.NotNull(hope);
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddEngineIDCtor();
    }
}