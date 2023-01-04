using DotCart.Abstractions.Behavior;
using DotCart.Drivers.RabbitMQ;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Engine.Context;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Drivers.Tests.ChangeDetails;

public class RMqEmitterDriverTests
    : RMqEmitterDriverTestsT<IRmqEmitterDriverT<Contract.ChangeDetails.Payload, EventMeta>,
        Contract.ChangeDetails.Payload, EventMeta>
{
    public RMqEmitterDriverTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddChangeDetailsSpoke();
    }
}