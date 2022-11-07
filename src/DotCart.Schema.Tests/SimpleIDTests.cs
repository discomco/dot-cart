using DotCart.TestEnv.Engine.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Schema.Tests;

public class SimpleIDTests: IoCTests
{
    protected NewSimpleID<SimpleEngineID> _newID;

    [Fact]
    public void ShouldResolveSimpleIDCtor()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var newSimpleId = Container.GetRequiredService<NewSimpleID<SimpleEngineID>>();
        // THEN
        Assert.NotNull(newSimpleId);
    }

    [Fact]
    public void ShouldCreateASimpleID()
    {
        // GIVEN
        Assert.NotNull(_newID);
        // WHEN
        var newID = _newID();
        // THEN
        Assert.NotNull(newID);
        Assert.Equal(Constants.EngineIDPrefix, newID.Prefix);
    }

    [Fact]
    public void ShouldCreateFromIdString()
    {
        // GIVEN
        Assert.NotNull(_newID);
        // WHEN
        var newID = _newID();
        var newerID = newID.Id().IDFromIdString();
        Assert.NotNull(newerID);
        Assert.Equal(newID.Id(), newerID.Id());
    }
    
    
    public SimpleIDTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void Initialize()
    {
        _newID = Container.GetRequiredService<NewSimpleID<SimpleEngineID>>();
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