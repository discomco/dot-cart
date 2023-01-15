using DotCart.Abstractions.Schema;
using DotCart.TestFirst.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Schema;

public class RootListItemTests : EntityTestsT<Contract.Schema.EngineID, Contract.Schema.EngineListItem>
{
    public RootListItemTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddTransient(_ => TestUtils.Schema.DocIDCtor)
            .AddRootListCtors();
    }

    protected override Contract.Schema.EngineListItem CreateEntity(IDCtorT<Contract.Schema.EngineID> idCtor)
    {
        return new Contract.Schema.EngineListItem
        {
            Id = _idCtor().Id(),
            Name = "Test Engine",
            Status = Contract.Schema.EngineStatus.Initialized,
            Power = 42
        };
    }
}