using DotCart.Abstractions.Behavior;
using DotCart.Drivers.NATS.TestFirst;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.Tests;

public class MyNATSListenerTests
    : NATSListenerTestsT<MyPayload, MetaB>
{
    public MyNATSListenerTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }
}
