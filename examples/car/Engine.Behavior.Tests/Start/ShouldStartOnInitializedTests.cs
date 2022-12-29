using DotCart.Core;
using DotCart.TestFirst.Actors;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Start;

[Name(Behavior.Start.OnInitialized_v1)]
public class ShouldStartOnInitializedTests
    : ChoreographyTestsT<
        Behavior.Initialize.IEvt,
        Behavior.Start.Cmd>
{
    public ShouldStartOnInitializedTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
        services
            .AddStartBehavior();
    }
}