using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using Engine.Context.Initialize;
using Microsoft.Extensions.DependencyInjection;
using IEvt = DotCart.Abstractions.Behavior.IEvt;

namespace Engine.Context.Common;

public delegate IEnumerable<ICmd> ScenarioGenerator<in TID>(TID ID)
    where TID : IID;

public delegate IEnumerable<IEvt> EventStreamGenerator<in TID>(TID ID)
    where TID : IID;

public static partial class Inject
{
    public static IServiceCollection AddInitializeWithThrottleUpEvents(this IServiceCollection services)
    {
        return services.AddTransient<EventStreamGenerator<Contract.Schema.EngineID>>(_ =>
            ScenariosAndStreams.InitializeWithThrottleUpEvents);
    }

    public static IServiceCollection AddInitializeEngineScenario(this IServiceCollection services)
    {
        return services.AddTransient<ScenarioGenerator<Contract.Schema.EngineID>>(_ =>
            ScenariosAndStreams.InitializeScenario);
    }
}

public static class ScenariosAndStreams
{
    public static IEnumerable<IEvt> InitializeWithThrottleUpEvents(Contract.Schema.EngineID ID)
    {
        var initPayload = Contract.Initialize.Payload.New(Contract.Schema.Details.New("New Engine"));
        var initEvt = Event.New(ID, Topics.EvtTopic, initPayload, EventMeta.New("", ID.Id()));
        initEvt.Version = 0;
        // AND
        var startPayload = Contract.Start.Payload.New;
        var startEvt = Event.New(ID, Start.Topics.EvtTopic, startPayload, EventMeta.New("", ID.Id()));
        startEvt.Version = 1;
        // AND
        var revs = RandomRevs(ID);
        var res = new List<IEvt> { initEvt, startEvt };
        res.AddRange(revs);

//        var throttleUpCmd = ThrottleUp.Cmd.New(_engineID, throttleUpPayload);
        // WHEN
        return res;
    }

    private static IEnumerable<IEvt> RandomRevs(ID ID)
    {
        var res = new List<IEvt>();
        var counter = Random.Shared.Next(4, 15);
        for (var i = 0; i < counter; i++)
        {
            var delta = Random.Shared.Next(-10, 10);
            var changeRpmPld = Contract.ChangeRpm.Payload.New(delta);
            var changeRpmEvt = Event.New(ID, ChangeRpm.Topics.EvtTopic, changeRpmPld, EventMeta.New("", ID.Id()));
            changeRpmEvt.Version = i + 2;
            res.Add(changeRpmEvt);
        }

        return res;
    }

    public static IEnumerable<ICmd> InitializeScenario(IID ID)
    {
        var initializePayload = Contract.Initialize.Payload.New(Contract.Schema.Details.New("New Engine"));
        var initializeCmd = Cmd.New(ID, initializePayload);
        return new[] { initializeCmd };
    }

    public static async Task<IEnumerable<IFeedback>> RunScenario(IAggregate aggregate,
        IEnumerable<ICmd> scenario)
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