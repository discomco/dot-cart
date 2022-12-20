using DotCart.Context.Behaviors;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Stop;

public class AggregateTests : AggregateTestsT<
    Schema.EngineID,
    Engine,
    TryCmdT<Behavior.Stop.Cmd, Engine>,
    ApplyEvtT<Engine, Behavior.Stop.IEvt>,
    Behavior.Stop.Cmd,
    Behavior.Stop.IEvt
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