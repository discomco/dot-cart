using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class StoreTestsT<TDbInfo> 
    : IoCTests
{
    protected StoreTestsT(ITestOutputHelper output, IoCTestContainer testEnv) 
        : base(output, testEnv)
    {
    }
    
    [Fact]
    public void ShouldKnowDbNameAtt()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var name = DbNameAtt.Get<TDbInfo>();
        // THEN
        Assert.NotEmpty(name);
    }
    
}