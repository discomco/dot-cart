using DotCart.TestEnv.Engine.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;


namespace DotCart.Schema.Tests;

public class IdentityTests : IdentityTests<EngineID>
{
    public IdentityTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void SetTestEnvironment()
    {}

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddIDCtor();
    }
}