using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeDetails;

public class FromNATSTests
    : NATSResponderTestsT<
        Context.ChangeDetails.Spoke,
        Context.ChangeDetails.FromNATS,
        Contract.ChangeDetails.Payload,
        EventMeta>
{
    public FromNATSTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddChangeDetailsSpoke();
    }
}