using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.ChangeRpm;

public class Evt2ListTests 
    : Evt2DocTestsT<
        Behavior.ChangeRpm.IEvt,
        Schema.EngineListID,
        Schema.EngineList,
        Contract.ChangeRpm.Payload,
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
            .AddTransient(_ => TestUtils.ChangeRpm.EvtCtor)
            .AddTransient(_ => TestUtils.ChangeRpm.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.ListIDCtor)
            .AddTransient(_ => TestUtils.Schema.FilledListCtor)
            .AddTransient(_ => TestUtils.Schema.MetaCtor)
            .AddChangeRpmProjectionFuncs();
    }

    protected override bool IsValidProjection(Schema.EngineList oldDoc, Schema.EngineList newDoc, Event evt)
    {
        if (newDoc.Items.All(item => item.Key != evt.AggregateId)) 
            return false;
        if (oldDoc.Items.All(item => item.Key != evt.AggregateId)) 
            return false;
        var d = evt.GetPayload<Contract.ChangeRpm.Payload>().Delta;
        return newDoc.Items[evt.AggregateId].Power - oldDoc.Items[evt.AggregateId].Power == d;
    }
}