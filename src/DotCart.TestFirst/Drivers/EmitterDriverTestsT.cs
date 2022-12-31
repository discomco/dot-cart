using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class EmitterDriverTestsT<TIFact, TPayload, TDriverMsg> 
    : BusDriverTestsB<TIFact, TPayload, TDriverMsg> 
    where TIFact : IFactB 
    where TPayload : IPayload 
    where TDriverMsg : class
{
    protected EmitterDriverTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }
    
    [Fact]
    public void ShouldResolveEmitterDriver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        using var driver = TestEnv.ResolveRequired<IEmitterDriverT<TDriverMsg,TPayload>>();
        // THEN
        Assert.NotNull(driver);
    }


    [Fact]
    public void ShouldResolveFact2Msg()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var fact2Msg = TestEnv.ResolveRequired<Fact2Msg<TDriverMsg, TPayload>>();
        // THEN
        Assert.NotNull(fact2Msg);
    }

}