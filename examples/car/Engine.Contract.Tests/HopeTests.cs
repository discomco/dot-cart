using DotCart.Contract.Schemas;
using DotCart.Core;
using DotCart.TestKit;
using Engine.Contract.Initialize;
using Engine.Contract.Schema;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Payload = Engine.Contract.Initialize.Payload;

namespace Engine.Contract.Tests;

public class HopeTests : IoCTests
{
    private NewID<EngineID> _newID;

    public HopeTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    [Fact]
    public void ShouldCreateInitializeHope()
    {
        // GIVEN
        var details = Details.New("A New Engine");
        var pl = Payload.New(details);
        // WHEN
        var engineID = _newID();
        var hope = Hope.New(engineID.Id(), pl.ToBytes());
        // THEN
        Assert.NotNull(hope);
    }

    protected override void Initialize()
    {
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
}