using DotCart.Abstractions.Behavior;
using DotCart.Core;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Funcs = Engine.TestUtils.Initialize.Funcs;

namespace Engine.Behavior.Tests.Initialize;

[Topic(Contract.Initialize.Topics.Evt_v1)]
public class EvtTests
    : EvtTestsT<Schema.EngineID, Contract.Initialize.Payload, MetaB>
{
    public EvtTests(ITestOutputHelper output, IoCTestContainer testEnv)
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
            .AddTransient(_ => Funcs.EvtCtor)
            .AddTransient(_ => Funcs.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.DocIDCtor);
    }
}