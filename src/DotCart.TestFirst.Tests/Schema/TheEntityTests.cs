using DotCart.TestFirst.Schema;
using DotCart.TestKit;
using DotCart.TestKit.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Tests.Schema;

public class TheEntityTests :
    EntityTestsT<
        TheSchema.EntityID, 
        TheSchema.Entity>
{
    public TheEntityTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddTheDocCtors();
    }
}