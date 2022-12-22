using DotCart.Context.Behaviors;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.ChangeDetails;

public class AggregateTests : AggregateTestsT<
    Schema.EngineID,
    Schema.Engine,
    TryCmdT<Behavior.ChangeDetails.Cmd, Schema.Engine>,
    ApplyEvtT<Schema.Engine, Behavior.ChangeDetails.IEvt>,
    Behavior.ChangeDetails.Cmd,
    Behavior.ChangeDetails.IEvt>
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
            .AddChangeDetailsBehavior();
    }
}