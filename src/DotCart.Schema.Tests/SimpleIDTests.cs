using DotCart.TestEnv.Engine.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Schema.Tests;

public class SimpleIDTests: IoCTests
{

    protected NewSimpleID<SimpleEngineID> _newEngineID;

    [Fact]
    public void ShouldResolveSimpleIDCtor()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var _newID = Container.GetRequiredService<NewSimpleID<SimpleEngineID>>();
        // THEN
        Assert.NotNull(_newID);
    }

    [Fact]
    public void ShouldCreateASimpleID()
    {
        // GIVEN
        Assert.NotNull(_newEngineID);
        // WHEN
        var _newID = _newEngineID();
        // THEN
        Assert.NotNull(_newID);
        Assert.Equal(Constants.EngineIDPrefix, _newID.Prefix);

    }
    
    
    public SimpleIDTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void Initialize()
    {
        _newEngineID = Container.GetRequiredService<NewSimpleID<SimpleEngineID>>();
    }

    protected override void SetTestEnvironment()
    {
    }
    
    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddSimpleIDCtor();
    }
}