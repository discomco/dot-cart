using DotCart.Client.Schemas;
using DotCart.Core;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Client.Initialize;
using Engine.Client.Schema;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Payload = Engine.Client.Initialize.Payload;

namespace Engine.Client.Tests;

public class InitializeContractTests : ContractTests<EngineID, IHope, IFact, Payload>
{
    public InitializeContractTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }


    protected override void Initialize()
    {
        base.Initialize();
        _newID = Container.GetRequiredService<NewID<EngineID>>();
    }


    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddEngineIDCtor();
    }


    public override void ShouldCreateNewPayload()
    {
        // GIVEN
        var details = Details.New("Payload Details");
        // WHEN
        var pl = Payload.New(details);
        // THEN
        Assert.NotNull(pl);
    }

    public override void ShouldCreateFactFromBytes()
    {
        // GIVEN
        var aggId = _newID();
        var details = Details.New("Payload Details");
        var payload = Payload.New(details);
        // WHEN
        var fact = Fact.New(aggId.Id(), payload.ToBytes());
        // THEN
        Assert.NotNull(fact);
    }

    public override void ShouldCreateFactFromPayload()
    {
        // GIVEN
        var aggId = _newID();
        var details = Details.New("New Engine");
        var payload = Payload.New(details);
        // WHEN
        var fact = Fact.New(aggId.Id(), payload);
        // THEN
        Assert.NotNull(fact);
    }

    public override void ShouldCreateHopeFromBytes()
    {
        // GIVEN
        var aggId = _newID();
        var details = Details.New("New Engine");
        var payload = Payload.New(details);
        // WHEN
        var hope = Hope.New(aggId.Id(), payload.ToBytes());
        // THEN
        Assert.NotNull(hope);
    }

    public override void ShouldCreateHopeFromPayload()
    {
        // GIVEN
        var aggId = _newID();
        var details = Details.New("New Engine");
        var payload = Payload.New(details);
        // WHEN
        var fact = Fact.New(aggId.Id(), payload);
        // THEN
        Assert.NotNull(fact);
    }
}