using DotCart.Context.Behaviors;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.ChangeRpm;

public class AggregateTests : AggregateTestsT<
    Schema.EngineID,
    Schema.Engine,
    TryCmdT<Behavior.ChangeRpm.Cmd, Schema.Engine>,
    ApplyEvtT<Schema.Engine, Behavior.ChangeRpm.IEvt>,
    Behavior.ChangeRpm.Cmd,
    Behavior.ChangeRpm.IEvt>
{
    public AggregateTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddEngineBehavior();
    }
}