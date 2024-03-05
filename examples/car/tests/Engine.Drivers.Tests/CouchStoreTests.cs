using DotCart.Drivers.CouchDB;
using DotCart.Drivers.CouchDB.TestFirst;
using DotCart.TestKit;
using DotCart.TestKit.Mocks;
using Engine.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Schema = Engine.Contract.Schema;


namespace Engine.Drivers.Tests;

public class CouchStoreTests
    : CouchStoreTestsT<TheContext.ICouchDocDbInfo, Schema.Engine, Schema.EngineID>
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
        services
            .AddDotCouch<TheContext.ICouchDocDbInfo, Schema.Engine, Schema.EngineID>()
            .AddTestDocCtors();
    }
}