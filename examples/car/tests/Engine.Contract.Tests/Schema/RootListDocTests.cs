using DotCart.TestFirst.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Schema;

public class RootListDocTests
    : ListDocTestsT<
        Contract.Schema.EngineListID,
        Contract.Schema.EngineList,
        Contract.Schema.EngineListItem>
{
    public RootListDocTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddRootListCtors();
    }
}