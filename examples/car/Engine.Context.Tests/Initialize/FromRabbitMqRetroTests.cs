using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class FromRabbitMqRetroTests
    : RetroListenerTestsT<Context.Initialize.Spoke,
        Context.Initialize.FromRabbitMqRetro,
        Contract.Initialize.Payload,
        MetaB,
        Context.Initialize.IRetroPipe,
        Schema.EngineID>
{
    public FromRabbitMqRetroTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
        TestUtils.Initialize
            .AddTestFuncs(services)
            .AddInitializeSpoke();
    }
}