using DotCart.Abstractions;
using DotCart.TestFirst.Delivery;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeRpm;

[Name(Context.ChangeRpm.Spoke_v1)]
public class SpokeTests
    : SpokeTestsT<Context.ChangeRpm.Spoke>
{
    public SpokeTests(ITestOutputHelper output, IoCTestContainer testEnv)
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
            .AddChangeRpmSpoke();
    }
}