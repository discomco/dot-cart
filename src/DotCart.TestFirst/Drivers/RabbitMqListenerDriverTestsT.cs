using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Defaults.RabbitMq;
using DotCart.TestKit;
using RabbitMQ.Client;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class RabbitMqListenerDriverTestsT<TFactPayload, TMeta>
    : ListenerDriverTestsT<IRmqListenerDriverT<TFactPayload>, TFactPayload, TMeta>
    where TFactPayload : IPayload
    where TMeta : IEventMeta
{
    protected RabbitMqListenerDriverTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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