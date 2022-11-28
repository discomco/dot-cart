using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Stop;

public class AggregateTests : AggregateTestsT<
    Schema.EngineID,
    Engine,
    Behavior.Stop.TryCmd,
    Behavior.Stop.ApplyEvt,
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