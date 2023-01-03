using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.TestUtils;

public delegate IEnumerable<ICmdB> ScenarioGenerator<in TID>(TID ID)
    where TID : IID;

public delegate IEnumerable<IEvtB> EventStreamGenerator<in TID>(TID ID)
    where TID : IID;

public static partial class Inject
{
    public static IServiceCollection AddInitializeWithChangeRpmEvents(this IServiceCollection services)
    {
        return services.AddTransient<EventStreamGenerator<Contract.Schema.EngineID>>(_ =>
            ScenariosAndStreams.InitializeWithChangeRpmEvents);
    }

    public static IServiceCollection AddInitializeEngineScenario(this IServiceCollection services)
    {
        return services.AddTransient<ScenarioGenerator<Contract.Schema.EngineID>>(_ =>
            ScenariosAndStreams.InitializeScenario);
    }
}

public static class ScenariosAndStreams
{
    public static IEnumerable<IEvtB> InitializeWithChangeRpmEvents(Contract.Schema.EngineID ID)
    {
        var initPayload = Contract.Initialize.Payload.New(Contract.Schema.Details.New("New Engine"));
        var initEvt = Behavior.Initialize._newEvt(ID, initPayload, Schema.MetaCtor(ID.Id()));
        initEvt.SetVersion(0);
        // AND
        var startPayload = Contract.Start.Payload.New;
        var startEvt = Behavior.Start._newEvt(ID, startPayload, Schema.MetaCtor(ID.Id()));
        startEvt.SetVersion(1);
        // AND
        var revs = RandomRevs(ID);
        var res = new List<IEvtB> { initEvt, startEvt };
        res.AddRange(revs);
        // WHEN
        return res;
    }

    private static IEnumerable<IEvtB> RandomRevs(IDB ID)
    {
        var res = new List<IEvtB>();
        var counter = Random.Shared.Next(4, 15);
        for (var i = 0; i < counter; i++)
        {
            var delta = Random.Shared.Next(-10, 10);
            var changeRpmPld = Contract.ChangeRpm.Payload.New(delta);
            var changeRpmEvt = Behavior.ChangeRpm._newEvt(
                ID,
                changeRpmPld,
                Schema.MetaCtor(ID.Id()));
            changeRpmEvt.SetVersion(i + 2);
            res.Add(changeRpmEvt);
        }

        return res;
    }

    public static IEnumerable<ICmdB> InitializeScenario(IDB ID)
    {
        var initializePayload = Contract.Initialize.Payload.New(Contract.Schema.Details.New("New Engine"));
        var initializeCmd = CmdT<Contract.Initialize.Payload, EventMeta>.New(
            ID,
            initializePayload,
            EventMeta.New(NameAtt.Get<IEngineAggregateInfo>(), ID.Id())
        );
        initializeCmd.SetID(ID);
        return new[] { initializeCmd };
    }

    public static async Task<IEnumerable<IFeedback>> RunScenario(IAggregate aggregate,
        IEnumerable<ICmdB> scenario)
    {
        var accu = new List<IFeedback>();
        foreach (var command in scenario)
        {
            var fbk = await aggregate.ExecuteAsync(command);
            accu.Add(fbk);
        }

        return accu;
    }
}