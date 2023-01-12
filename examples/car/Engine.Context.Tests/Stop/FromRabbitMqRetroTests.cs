using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Stop;

public class FromRabbitMqRetroTests
    : RetroListenerTestsT<
        Context.Stop.Spoke,
        Context.Stop.FromRabbitMqRetro,
        Contract.Stop.Payload,
        MetaB,
        Context.Stop.IRetroInPipe,
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
        TestUtils.Stop
            .AddTestFuncs(services)
            .AddStopSpoke();
    }
}