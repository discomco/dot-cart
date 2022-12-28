using DotCart.Abstractions.Schema;
using DotCart.TestFirst.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Schema;

[IDPrefix(IDConstants.EngineListIDPrefix)]
public class RootListIDTests : IDTestsT<Contract.Schema.EngineListID>
{
    public RootListIDTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddListIDCtor();
    }
}