using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using RabbitMQ.Client;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;


public abstract class RMqEmitterDriverTestsT<TIFact, TPayload, TMsg>
    : EmitterDriverTestsT<TIFact, TPayload, TMsg> 
    where TIFact : IFactB 
    where TPayload : IPayload 
    where TMsg : class
{
    protected RMqEmitterDriverTestsT(ITestOutputHelper output, IoCTestContainer testEnv) 
        : base(output, testEnv)
    {
    }
    
    [Fact]
    public void ShouldResolveConnectionFactory()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var connFact = TestEnv.ResolveRequired<IConnectionFactory>();
        // THEN
        Assert.NotNull(connFact);
    }

}


public abstract class RMqListenerDriverTests<TIFact, TPayload, TMsg> 
    : ListenerDriverTestsT<TIFact, TPayload, TMsg> 
    where TIFact : IFactB 
    where TPayload : IPayload 
    where TMsg : class
{
    protected RMqListenerDriverTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveConnectionFactory()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var connFact = TestEnv.ResolveRequired<IConnectionFactory>();
        // THEN
        Assert.NotNull(connFact);
    }
    
    


}