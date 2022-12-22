using DotCart.Context.Behaviors;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Start;

public class AggregateTests : AggregateTestsT<
    Schema.EngineID,
    Schema.Engine,
    TryCmdT<Behavior.Start.Cmd, Schema.Engine>,
    ApplyEvtT<Schema.Engine, Behavior.Start.IEvt>,
    Behavior.Start.Cmd,
    Behavior.Start.IEvt
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