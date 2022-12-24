using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Start;

public class Evt2ListTests : 
    Evt2DocTestsT<
        Behavior.Start.IEvt, 
        Schema.EngineListID, 
        Schema.EngineList, 
        Contract.Start.Payload, 
        EventMeta>
{
    public Evt2ListTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddTransient(_ => TestUtils.Schema.FilledListCtor)
            .AddTransient(_ => TestUtils.Start.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.MetaCtor)
            .AddTransient(_ => TestUtils.Start.EvtCtor)
            .AddStartProjectionFuncs()
            .AddListIDCtor();
    }

    protected override bool IsValidProjection(Schema.EngineList oldDoc, Schema.EngineList newDoc, Event evt)
    {
        return newDoc.Items.Any(it => it.Key == evt.AggregateId) 
               && newDoc.Items[evt.AggregateId].Status.HasFlagFast(Schema.EngineStatus.Started);
    }
}