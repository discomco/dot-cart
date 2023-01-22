using System.Threading.Tasks;
using DotCart.Context.Behavior;
using DotCart.TestKit;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Behavior;

public abstract class FullBehaviorTestsT : IoCTests
{
    private IAggregateBuilder _aggBuilder;

    protected FullBehaviorTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public async Task ShouldResolveAggregateBuilder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _aggBuilder = TestEnv.ResolveRequired<IAggregateBuilder>();
        // THEN
        Assert.NotNull(_aggBuilder);
    }


    [Fact]
    public async Task ShouldBuildAggregate()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        _aggBuilder = TestEnv.ResolveRequired<IAggregateBuilder>();
        Assert.NotNull(_aggBuilder);
        // WHEN
        var agg1 = _aggBuilder.Build();
        var agg2 = _aggBuilder.Build();
        // THEN
        Assert.NotNull(agg1);
        Assert.NotNull(agg2);
        Assert.NotSame(agg1, agg2);
    }
}