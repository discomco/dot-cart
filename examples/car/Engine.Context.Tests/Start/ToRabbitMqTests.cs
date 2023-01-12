using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Start;

public class ToRabbitMqTests
    : EmitterTestsT<
        Context.Start.Spoke,
        Context.Start.ToRabbitMq,
        Contract.Start.Payload,
        MetaB,
        Schema.EngineID>
{
    public ToRabbitMqTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
        TestUtils.Start
            .AddTestFuncs(services)
            .AddStartSpoke();
    }
}