using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.ChangeDetails;

public class Evt2ListTests 
    : Evt2DocTestsT<
        Behavior.ChangeDetails.IEvt, 
        Schema.EngineListID,
        Schema.EngineList, 
        Contract.ChangeDetails.Payload, 
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
            .AddChangeDetailsProjectionFuncs()
            .AddListIDCtor()
            .AddTransient(_ => TestUtils.Schema.FilledListCtor)
            .AddTransient(_ => TestUtils.ChangeDetails.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.MetaCtor)
            .AddTransient(_ => TestUtils.ChangeDetails.EvtCtor);
    }

    protected override bool IsValidProjection(Schema.EngineList oldDoc, Schema.EngineList newDoc, Event evt)
    {
        return oldDoc.Items.Count == newDoc.Items.Count
               && newDoc.Items[evt.AggregateId].Name == evt.GetPayload<Contract.ChangeDetails.Payload>().Details.Name;
    }
}