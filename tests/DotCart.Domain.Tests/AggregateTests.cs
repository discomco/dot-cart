using DotCart.Schema.Tests;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Domain.Tests;

public class AggregateTests : IoCTests
{

    private EngineID _engineId;
    
    [Fact]
    public void ShouldBeAbleToCreateEngineID()
    {
        // GIVEN
        // WHEN
        var ID = EngineID.New;
        // THEN
        Assert.NotNull(ID);
    }

    public AggregateTests(ITestOutputHelper output, IoCTestContainer container) 
        : base(output, container)
    {
    }

    protected override void Initialize()
    {
        
    }

    protected override void SetTestEnvironment()
    {
        
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services.AddTransient<IAggregate, Aggregate<Engine>>();
    }
}