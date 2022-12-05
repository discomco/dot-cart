using DotCart.Context.Behaviors;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.ChangeRpm;

public class AggregateTests : AggregateTestsT<
    Schema.EngineID,
    Engine,
    TryCmdT<Behavior.ChangeRpm.Cmd, Engine>,
    ApplyEvtT<Engine, Behavior.ChangeRpm.Evt>,
    Behavior.ChangeRpm.Cmd,
    Behavior.ChangeRpm.Evt>
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