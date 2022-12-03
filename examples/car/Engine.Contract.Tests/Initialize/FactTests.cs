using DotCart.Core;
using DotCart.TestFirst.Contract;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Initialize;


[Topic(Contract.Initialize.Topics.Fact_v1)]
public class FactTests : FactTestsT<Contract.Schema.EngineID, Contract.Initialize.Fact, Contract.Initialize.Payload> 
{
    public FactTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetTestEnvironment()
    {
        
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddTransient(_ => TestUtils.Initialize.FactCtor)
            .AddTransient(_ => TestUtils.Schema.IDCtor)
            .AddTransient(_ => TestUtils.Initialize.PayloadCtor);
    }
}