using DotCart.Core;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.ChangeDetails;

[Name(Behavior.ChangeDetails.OnInitialized_v1)]
public class OnInitializedTests
    : PolicyTestsT<
        Behavior.ChangeDetails.OnInitialized,
        Behavior.Initialize.IEvt,
        Behavior.ChangeDetails.Cmd>
{
    public OnInitializedTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void Initialize()
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddChangeDetailsBehavior();
    }
}