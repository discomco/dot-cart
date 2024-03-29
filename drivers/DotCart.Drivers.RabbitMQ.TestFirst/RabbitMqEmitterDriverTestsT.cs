using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using RabbitMQ.Client;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.Drivers.RabbitMQ.TestFirst;

public abstract class RabbitMqEmitterDriverTestsT<TEmitterDriver, TPayload, TMeta>
    : EmitterDriverTestsT<TEmitterDriver, TPayload, TMeta>
    where TPayload : IPayload
    where TMeta : IMetaB
    where TEmitterDriver : IEmitterDriverB
{
    protected RabbitMqEmitterDriverTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
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