using DotCart.Abstractions.Behavior;
using DotCart.Core;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Start;

[Topic(Contract.Start.Topics.Evt_v1)]
public class EvtTests : EvtTestsT<Schema.EngineID, Contract.Start.Payload, MetaB>
{
    public EvtTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddTransient(_ => TestUtils.Start.EvtCtor)
            .AddTransient(_ => TestUtils.Start.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.DocIDCtor);
    }
}