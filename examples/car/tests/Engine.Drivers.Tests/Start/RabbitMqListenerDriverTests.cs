using DotCart.Abstractions.Behavior;
using DotCart.Drivers.RabbitMQ.TestFirst;
using DotCart.TestKit;
using Engine.Context;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Drivers.Tests.Start;

public class RabbitMqListenerDriverTests
    : RabbitMqListenerDriverTestsT<Contract.Start.Payload, MetaB>
{
    public RabbitMqListenerDriverTests(ITestOutputHelper output, IoCTestContainer testEnv)
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
            .AddStartSpoke();
    }
}