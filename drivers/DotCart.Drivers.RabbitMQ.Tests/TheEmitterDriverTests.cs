using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.RabbitMQ.Tests;

public class TheEmitterDriverTests :
    RMqEmitterDriverTestsT<TheContract.Payload, TheContract.Meta>
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
            .AddRabbitMqEmitterDriverT<TheContract.Payload, TheContract.Meta>()
            .AddSingletonRMq()
            .AddTheACLFuncs();
    }
}