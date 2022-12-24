using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.ChangeRpm;

public class Evt2DocTests
    : Evt2DocTestsT<
        Behavior.ChangeRpm.IEvt,
        Schema.EngineID,
        Schema.Engine,
        Contract.ChangeRpm.Payload,
        EventMeta>
{
    public Evt2DocTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddRootIDCtor()
            .AddChangeRpmProjectionFuncs()
            .AddTransient(_ => TestUtils.ChangeRpm.EvtCtor)
            .AddTransient(_ => TestUtils.ChangeRpm.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.DocCtor)
            .AddTransient(_ => TestUtils.Schema.MetaCtor);
    }

    protected override bool IsValidProjection(Schema.Engine oldDoc, Schema.Engine newDoc, Event evt)
    {
        return newDoc.Power == oldDoc.Power + evt.GetPayload<Contract.ChangeRpm.Payload>().Delta;
    }
}