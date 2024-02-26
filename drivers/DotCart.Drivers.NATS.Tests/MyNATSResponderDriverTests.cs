using DotCart.Drivers.NATS.TestFirst;
using DotCart.Logging;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.Tests;

public class MyNATSResponderDriverTests
    : NATSResponderDriverTestsT<MyReq>
{
    public MyNATSResponderDriverTests(ITestOutputHelper output, IoCTestContainer testEnv)
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
            .AddCoreNATS()
            .AddSeriloggersFromCode()
            .AddNATSResponderDriverT<MyReq>();
    }
}