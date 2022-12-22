using DotCart.Context.Behaviors;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Initialize;

public class AggregateTests : AggregateTestsT<
    Schema.EngineID,
    Schema.Engine,
    TryCmdT<Behavior.Initialize.Cmd, Schema.Engine>,
    ApplyEvtT<Schema.Engine, Behavior.Initialize.IEvt>,
    Behavior.Initialize.Cmd,
    Behavior.Initialize.IEvt
>
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