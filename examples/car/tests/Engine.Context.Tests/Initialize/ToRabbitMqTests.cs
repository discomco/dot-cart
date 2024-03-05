using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Funcs = Engine.TestUtils.Initialize.Funcs;

namespace Engine.Context.Tests.Initialize;

public class ToRabbitMqTests
    : EmitterTestsT<Context.Initialize.Spoke, Context.Initialize.ToRabbitMq, Contract.Initialize.Payload, MetaB,
        Schema.EngineID>
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
        Funcs
            .AddTestFuncs(services)
            .AddInitializeSpoke();
    }
}