using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using RabbitMQ.Client;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class RabbitMqDriverTests<TIFact, TPayload, TMsg> 
    : AsyncInfraDriverTestsT<TIFact, TPayload, TMsg> 
    where TIFact : IFactB 
    where TPayload : IPayload 
    where TMsg : class
{
    protected RabbitMqDriverTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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