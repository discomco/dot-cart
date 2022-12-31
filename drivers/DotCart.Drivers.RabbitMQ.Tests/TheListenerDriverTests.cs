using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.RabbitMQ.Tests;

public class TheListenerDriverTests
    : RMqListenerDriverTests<
        TheContract.IFact,
        TheContract.Payload>
{
    public TheListenerDriverTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddRabbitMqListenerDriverT<TheContract.IFact, TheContract.Payload>()
            .AddSingletonRMq()
            .AddTheACLFuncs();
    }
}