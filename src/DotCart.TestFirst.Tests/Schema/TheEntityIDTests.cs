using DotCart.Abstractions.Schema;
using DotCart.TestFirst.Schema;
using DotCart.TestKit;
using DotCart.TestKit.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Tests.Schema;

[IDPrefix(TheConstants.EntityIDPrefix)]
public class TheEntityIDTests
    : IDTestsT<TheSchema.EntityID>
{
    public TheEntityIDTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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