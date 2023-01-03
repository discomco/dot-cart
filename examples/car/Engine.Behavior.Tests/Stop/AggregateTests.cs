using DotCart.Abstractions.Behavior;
using DotCart.Context.Behaviors;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Stop;

public class AggregateTests : AggregateTestsT<
    Schema.EngineID,
    Schema.Engine,
    Contract.Stop.Payload,
    EventMeta
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