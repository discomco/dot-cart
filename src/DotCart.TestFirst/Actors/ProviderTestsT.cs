using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class ProviderTestsT<TProvider, TQuery>
    : IoCTests
    where TProvider : IProviderT<TQuery>
    where TQuery : IQueryB
{
    protected ProviderTestsT(ITestOutputHelper output, IoCTestContainer testEnv) 
        : base(output, testEnv)
    {}

    [Fact]
    public void ShouldResolveProvider()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var provider = TestEnv.ResolveRequired<IProviderT<TQuery>>();
        // THEN
        Assert.NotNull(provider);
        Assert.IsType<TProvider>(provider);
    }
    
    
}