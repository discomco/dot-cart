using DotCart.Drivers.RabbitMQ.TestFirst;
using DotCart.TestKit;
using DotCart.TestKit.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.RabbitMQ.Tests;

public class TheListenerDriverTests
    : RabbitMqListenerDriverTestsT<TheContract.Payload, TheContract.Meta>
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
            .AddRabbitMqListenerDriverT<TheContract.Payload, TheContract.Meta>()
            .AddSingletonRMq()
            .AddTheACLFuncs();
    }
}