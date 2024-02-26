using DotCart.Drivers.CouchDB;
using DotCart.Drivers.CouchDB.TestFirst;
using DotCart.TestKit;
using DotCart.TestKit.Mocks;
using Engine.Context;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Inject = Engine.TestUtils.Inject;


namespace Engine.Drivers.Tests;

public class CouchStoreTests
    : CouchStoreTestsT<ICouchDocDbInfo, Schema.Engine, Schema.EngineID>
{
    public CouchStoreTests(ITestOutputHelper output, IoCTestContainer testEnv)
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

        Inject.AddTestDocCtors(services
            .AddDotCouch<TheContext.ICouchDocDbInfo, Schema.Engine, Schema.EngineID>());
    }
}