using DotCart.Drivers.CouchDB;
using DotCart.Drivers.CouchDB.TestFirst;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Drivers.Tests;

public class CouchDocStoreTests
: CouchStoreTestsT<Context.ICouchDocDbInfo, 
    Schema.Engine, 
    Schema.EngineID>
{
    public CouchDocStoreTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddRootDocCtors()
            .AddDotCouch<Context.ICouchDocDbInfo, Schema.Engine, Schema.EngineID>();
    }
}