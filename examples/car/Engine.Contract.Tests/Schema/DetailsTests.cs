using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestFirst.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Schema;

public class DetailsTests : ValueObjectTestsT<Contract.Schema.Details>
{
    

    public DetailsTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddRootDocCtors();
    }

    protected override Contract.Schema.Details CreateValueObject()
    {
        return Contract.Schema.Details.New("John Lennon", "John Lennon of the Beatles");
    }
}