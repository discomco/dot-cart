using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using RabbitMQ.Client;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class RMqEmitterDriverTestsT<TPayload, TMeta>
    : EmitterDriverTestsT<TPayload, TMeta>
    where TPayload : IPayload
    where TMeta : IEventMeta
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