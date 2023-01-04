using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeRpm;

public class ResponderTests
    : NATSResponderTestsT<
        Context.ChangeRpm.Spoke,
        Context.ChangeRpm.FromNATS,
        Contract.ChangeRpm.Payload,
        EventMeta>
{
    public ResponderTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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