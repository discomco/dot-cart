using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Core;
using DotCart.Abstractions.Schema;
using DotCart.TestKit.Mocks;
using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.TestUtils;

public static partial class Inject
{
    public static IServiceCollection AddInitializeWithChangeRpmEvents(this IServiceCollection services)
    {
        return services.AddTransient<EventStreamGenFuncT<Contract.Schema.EngineID>>(_ =>
            ScenariosAndStreams.InitializeWithChangeRpmEvents);
    }

    public static IServiceCollection AddInitializeEngineScenario(this IServiceCollection services)
    {
        return services.AddTransient<ScenarioGenFuncT<Contract.Schema.EngineID>>(_ =>
            ScenariosAndStreams.InitializeScenario);
    }
}

public static class ScenariosAndStreams
{
    public static IEnumerable<IEvtB> InitializeWithChangeRpmEvents(Contract.Schema.EngineID ID)
    {
        var initPayload = Contract.Initialize.Payload.New(Contract.Schema.Details.New("New Engine"));
        var initEvt = Behavior.Initialize._newEvt(ID, initPayload.ToBytes(), Schema.MetaCtor(ID.Id()).ToBytes());
        initEvt.SetVersion(0);
        // AND
        var startPayload = Contract.Start.Payload.New;
        var startEvt = Behavior.Start._newEvt(ID, startPayload.ToBytes(), Schema.MetaCtor(ID.Id()).ToBytes());
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
                changeRpmPld.ToBytes(),
                Schema.MetaCtor(ID.Id()).ToBytes());
            changeRpmEvt.SetVersion(i + 2);
            res.Add(changeRpmEvt);
        }

        return res;
    }

    public static IEnumerable<ICmdB> InitializeScenario(IDB ID)
    {
        var initializePayload = Contract.Initialize.Payload.New(Contract.Schema.Details.New("New Engine"));
        var initializeCmd = Command.New<Contract.Initialize.Payload>(
            ID,
            initializePayload.ToBytes(),
            MetaB.New(NameAtt.Get<IEngineAggregateInfo>(), ID.Id()).ToBytes()
        );
        //        initializeCmd.SetID(ID);
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