using DotCart.TestEnv.Engine;
using DotCart.TestEnv.Engine.Schema;
using DotCart.TestFirst;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Contract.Tests.Engine;

public class StartContractTests: ContractTests<SimpleEngineID,Start.Hope, Start.Fact, Start.Payload>
{
    public StartContractTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
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
        var fact = Start.Fact.New(ID.Value, pl.ToBytes());
        // THEN
        Assert.NotNull(fact);
    }

    public override void ShouldCreateFactFromPayload()
    {
        var pl = Start.Payload.New;
        var ID = _newID();
        // WHEN
        var fact = Start.Fact.New(ID.Value, pl);
        // THEN
        Assert.NotNull(fact);

    }

    public override void ShouldCreateHopeFromBytes()
    {
        var pl = Start.Payload.New;
        var ID = _newID();
        // WHEN
        var hope = Start.Hope.New(ID.Value, pl.ToBytes());
        // THEN
        Assert.NotNull(hope);

    }

    public override void ShouldCreateHopeFromPayload()
    {
        var pl = Start.Payload.New;
        var ID = _newID();
        // WHEN
        var hope = Start.Hope.New(ID.Value, pl);
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