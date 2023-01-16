using DotCart.Drivers.CouchDB;
using DotCart.Drivers.CouchDB.TestFirst;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Drivers.Tests;

public class CouchListStoreTests
: CouchDocStoreTestsT<Context.ICouchListDbInfo, Schema.EngineList, Schema.EngineListID>
{
    public CouchListStoreTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddRootListCtors()
            .AddDotCouch<Context.ICouchListDbInfo, Schema.EngineList, Schema.EngineListID>();
    }
}