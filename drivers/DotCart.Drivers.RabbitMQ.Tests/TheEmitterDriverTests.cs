using DotCart.Defaults.RabbitMq;
using DotCart.Drivers.RabbitMQ.TestFirst;
using DotCart.TestKit;
using DotCart.TestKit.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.RabbitMQ.Tests;

public class TheEmitterDriverTests :
    RabbitMqEmitterDriverTestsT<IRmqEmitterDriverT<TheContract.Payload, TheContract.Meta>, TheContract.Payload,
        TheContract.Meta>
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