using DotCart.TestEnv.Engine.Schema;
using DotCart.TestFirst;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;


namespace DotCart.Schema.Tests;

public class IDTests : IDTests<EngineID>
{
    public IDTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
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