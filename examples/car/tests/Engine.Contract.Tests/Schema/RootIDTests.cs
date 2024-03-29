using DotCart.Schema;
using DotCart.TestFirst.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Schema;

[IDPrefix(IDConstants.EngineIDPrefix)]
public class RootIDTests
    : IDTestsT<Contract.Schema.EngineID>
{
    public RootIDTests(ITestOutputHelper output, IoCTestContainer testEnv)
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
            .AddRootIDCtor();
    }
}