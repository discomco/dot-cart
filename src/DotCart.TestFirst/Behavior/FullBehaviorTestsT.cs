using DotCart.Abstractions.Behavior;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Behavior;

public abstract class FullBehaviorTestsT<TAggregate> : IoCTests
    where TAggregate : IAggregate
{
    protected FullBehaviorTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public async Task ShouldResolveExchange()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        
        // THEN
    }

}