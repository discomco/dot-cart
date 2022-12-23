using DotCart.Abstractions.Behavior;
using DotCart.Core;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.ChangeRpm;

[Topic(Behavior.ChangeRpm.Topics.Evt_v1)]
public class EvtTests : EvtTestsT<Schema.EngineID, Behavior.ChangeRpm.IEvt, Contract.ChangeRpm.Payload, EventMeta>
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
            .AddTransient(_ => TestUtils.ChangeRpm.PayloadCtor)
            .AddTransient(_ => TestUtils.ChangeRpm.EvtCtor)
            .AddTransient(_ => TestUtils.Schema.DocIDCtor);
    }
}