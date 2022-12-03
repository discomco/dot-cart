using DotCart.Core;
using DotCart.TestFirst.Contract;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Stop;

[Topic(Contract.Stop.Topics.Fact_v1)]
public class FactTests : FactTestsT<Contract.Schema.EngineID, Contract.Stop.Fact, Contract.Stop.Payload>
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
            .AddTransient(_ => TestUtils.Schema.IDCtor)
            .AddTransient(_ => TestUtils.Stop.PayloadCtor)
            .AddTransient(_ => TestUtils.Stop.FactCtor);
    }
}