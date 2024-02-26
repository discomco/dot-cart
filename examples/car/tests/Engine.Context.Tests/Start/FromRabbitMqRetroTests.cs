using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Start;

public class FromRabbitMqRetroTests
    : RetroListenerTestsT<Context.Start.Spoke, Context.Start.FromRabbitMqRetro, Contract.Start.Payload,
        MetaB, Context.Start.IRetroPipe, Schema.EngineID>
{
    public FromRabbitMqRetroTests(ITestOutputHelper output, IoCTestContainer testEnv)
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
        TestUtils.Start
            .AddTestFuncs(services)
            .AddStartSpoke();
    }
}