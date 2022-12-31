using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.RabbitMQ.Tests;

public class TheEmitterDriverTests :
    RMqEmitterDriverTestsT<
        TheContract.IFact,
        TheContract.Payload>
{
    public TheEmitterDriverTests(ITestOutputHelper output, IoCTestContainer testEnv)
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
            .AddRabbitMqEmitterDriverT<TheContract.IFact, TheContract.Payload>()
            .AddSingletonRMq()
            .AddTheACLFuncs();
    }
}