using DotCart.Abstractions.Behavior;
using DotCart.Core;
using DotCart.TestFirst.Contract;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.ChangeDetails;

[Topic(Contract.ChangeDetails.Topics.Fact_v1)]
public class FactTests : FactTestsT<Contract.Schema.EngineID, Contract.ChangeDetails.Payload, Meta>
{
    public FactTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddTransient(_ => TestUtils.Schema.MetaCtor)
            .AddTransient(_ => TestUtils.ChangeDetails.FactCtor)
            .AddTransient(_ => TestUtils.ChangeDetails.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.DocIDCtor);
    }
}