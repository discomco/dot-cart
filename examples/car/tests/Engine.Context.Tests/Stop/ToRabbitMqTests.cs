using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Stop;

public class ToRabbitMqTests
    : EmitterTestsT<Context.Stop.Spoke, Context.Stop.ToRabbitMq, Contract.Stop.Payload,
        MetaB, Schema.EngineID>
{
    public ToRabbitMqTests(ITestOutputHelper output, IoCTestContainer testEnv)
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
        TestUtils.Stop
            .AddTestFuncs(services)
            .AddStopSpoke();
    }
}