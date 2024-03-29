using DotCart.Abstractions.Behavior;
using DotCart.Core;
using DotCart.TestFirst.Contract;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.ChangeRpm;

[Topic(Contract.ChangeRpm.Topics.Fact_v1)]
public class FactTests : FactTestsT<
    Contract.Schema.EngineID,
    Contract.ChangeRpm.Payload,
    MetaB>
{
    public FactTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddTransient(_ => TestUtils.Schema.MetaCtor)
            .AddTransient(_ => TestUtils.ChangeRpm.FactCtor)
            .AddTransient(_ => TestUtils.ChangeRpm.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.DocIDCtor);
    }
}