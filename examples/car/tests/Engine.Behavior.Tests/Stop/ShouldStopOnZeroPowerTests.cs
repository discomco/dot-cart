using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Stop;

[Name(Behavior.Stop.OnZeroPower_v1)]
public class ShouldStopOnZeroPowerTests
    : ChoreographyTestsT<Contract.Stop.Payload, Contract.ChangeRpm.Payload, MetaB>
{
    public ShouldStopOnZeroPowerTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
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
            .AddStopBehavior();
    }
}