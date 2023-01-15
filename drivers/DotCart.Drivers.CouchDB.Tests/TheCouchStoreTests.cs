using DotCart.Drivers.CouchDB.TestFirst;
using DotCart.TestKit;
using DotCart.TestKit.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.CouchDB.Tests;

public class TheCouchStoreTests
    : CouchStoreTestsT<TheContext.ICouchDocDbInfo, TheSchema.Doc, TheSchema.ID>
{
    public TheCouchStoreTests(ITestOutputHelper output, IoCTestContainer testEnv)
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
            .AddTheDocCtors()
            .AddDotCouch<TheContext.ICouchDocDbInfo, TheSchema.Doc, TheSchema.ID>();
        //.AddCouchDBStore<TheContext.ICouchDbInfo, TheSchema.ID, TheSchema.Doc>();
    }
}