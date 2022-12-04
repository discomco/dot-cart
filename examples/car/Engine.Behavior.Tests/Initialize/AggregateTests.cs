using DotCart.TestFirst;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Initialize;

public class AggregateTests : AggregateTestsT<
    Schema.EngineID,
    Engine,
    Behavior.Initialize.TryCmd,
    Behavior.Initialize.ApplyEvt,
    Behavior.Initialize.Cmd,
    Behavior.Initialize.Evt
>
{
    public AggregateTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddEngineBehavior();
    }
}