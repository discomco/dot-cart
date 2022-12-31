using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class ListenerDriverTestsT<TIFact, TPayload, TDriverMsg> 
    : BusDriverTestsB<TIFact, TPayload, TDriverMsg> 
    where TIFact : IFactB 
    where TPayload : IPayload 
    where TDriverMsg : class
{
    protected ListenerDriverTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }
    
    [Fact]
    public void ShouldResolveListenerDriver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var driver = TestEnv.ResolveRequired<IListenerDriverT<TIFact, TDriverMsg>>();
        // THEN
        Assert.NotNull(driver);
    }

    
    [Fact]
    public void ShouldResolveMsg2Fact()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var msg2Fact = TestEnv.ResolveRequired<Msg2Fact<TPayload, TDriverMsg>>();
        // THEN
        Assert.NotNull(msg2Fact);
    }
}