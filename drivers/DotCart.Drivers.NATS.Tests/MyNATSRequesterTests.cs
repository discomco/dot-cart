using DotCart.Drivers.NATS.TestFirst;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.Tests;

public class MyNATSRequesterTests
    : NATSRequesterTestsT<MyReq>
{
    public MyNATSRequesterTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    protected override void Initialize()
    {
    }

    protected override void SetEnVars()
    {
    }
}
