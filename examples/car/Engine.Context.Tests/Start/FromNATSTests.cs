using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Start;

public class FromNATSTests : NATSResponderTestsT<
    Context.Start.Spoke,
    Context.Start.FromNATS,
    Contract.Start.Payload,
    MetaB>
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
            .AddStartSpoke();
    }
}