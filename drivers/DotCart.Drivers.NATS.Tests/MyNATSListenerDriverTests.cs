using DotCart.Abstractions.Behavior;
using DotCart.Drivers.NATS.TestFirst;
using DotCart.Drivers.Serilog;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.Tests;

public class MyNATSListenerDriverTests
    : NATSListenerDriverTestsT<MyPayload, MetaB>
{
    public MyNATSListenerDriverTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    protected override void Initialize()
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddSeriloggersFromCode()
            .AddNATSListenerDriverT<MyPayload, MetaB>();
    }
}
