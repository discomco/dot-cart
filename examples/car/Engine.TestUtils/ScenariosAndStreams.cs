using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.TestUtils;

public delegate IEnumerable<ICmd> ScenarioGenerator<in TID>(TID ID)
    where TID : IID;

public delegate IEnumerable<IEvt> EventStreamGenerator<in TID>(TID ID)
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
    public static IEnumerable<IEvt> InitializeWithChangeRpmEvents(Contract.Schema.EngineID ID)
    {
        var initPayload = Contract.Initialize.Payload.New(Contract.Schema.Details.New("New Engine"));
        var initEvt = Behavior.Initialize.Evt.New(ID, initPayload);
        initEvt.SetVersion(0);
        // AND
        var startPayload = Contract.Start.Payload.New;
        var startEvt = Behavior.Start.Evt.New(ID, startPayload);
        startEvt.SetVersion(1);
        // AND
        var revs = RandomRevs(ID);
        var res = new List<IEvt> { initEvt, startEvt };
        res.AddRange(revs);
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
            var changeRpmEvt = Behavior.ChangeRpm.Evt.New(
                ID,
                changeRpmPld);
            changeRpmEvt.SetVersion(i + 2);
            res.Add(changeRpmEvt);
        }

        return res;
    }

    public static IEnumerable<ICmd> InitializeScenario(ID ID)
    {
        var initializePayload = Contract.Initialize.Payload.New(Contract.Schema.Details.New("New Engine"));
        var initializeCmd = Behavior.Initialize.Cmd.New(initializePayload);
        initializeCmd.SetID(ID);
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