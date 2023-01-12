using DotCart.Abstractions.Behavior;
using DotCart.Defaults.RabbitMq;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Engine.Context;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Drivers.Tests.Stop;

public class RabbitMqEmitterDriverTests
    : RabbitMqEmitterDriverTestsT<IRmqEmitterDriverT<Contract.Stop.Payload, MetaB>, Contract.Stop.Payload,
        MetaB>
{
    public RabbitMqEmitterDriverTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddStopSpoke();
    }
}