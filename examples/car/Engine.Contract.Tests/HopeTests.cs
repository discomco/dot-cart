using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests;

public class HopeTests : IoCTests
{
    private NewID<Schema.EngineID> _newID;

    public HopeTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldCreateInitializeHope()
    {
        // GIVEN
        var details = Schema.Details.New("A New Engine");
        var pl = Contract.Initialize.Payload.New(details);
        // WHEN
        var engineID = _newID();
        var hope = Contract.Initialize.Hope.New(engineID.Id(), pl.ToBytes());
        // THEN
        Assert.NotNull(hope);
    }

    protected override void Initialize()
    {
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
}