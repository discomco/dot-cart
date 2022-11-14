using DotCart.Context.Abstractions;
using DotCart.Contract.Dtos;
using DotCart.Contract.Schemas;
using Engine.Context.Initialize;
using Engine.Contract.Initialize;
using Engine.Contract.Schema;
using Microsoft.Extensions.DependencyInjection;
using IEvt = DotCart.Context.Abstractions.IEvt;
using Topics = Engine.Context.Initialize.Topics;

namespace Engine.Context.Common;

public delegate IEnumerable<ICmd> ScenarioGenerator<in TID>(TID ID)
    where TID : IID;

public delegate IEnumerable<IEvt> EventStreamGenerator<in TID>(TID ID)
    where TID : IID;

public static partial class Inject
{
    public static IServiceCollection AddInitializeEngineWithThrottleUpStream(this IServiceCollection services)
    {
        return services.AddTransient<EventStreamGenerator<EngineID>>(_ =>
            ScenariosAndStreams.InitializeEngineWithThrottleUpEventStream);
    }

    public static IServiceCollection AddInitializeEngineScenario(this IServiceCollection services)
    {
        return services.AddTransient<ScenarioGenerator<EngineID>>(_ => ScenariosAndStreams.InitializeScenario);
    }
}

public static class ScenariosAndStreams
{
    public static IEnumerable<IEvt> InitializeEngineWithThrottleUpEventStream(EngineID ID)
    {
        var initPayload = Payload.New(Details.New("New Engine"));
        var initEvt = Event.New(ID, Topics.EvtTopic, initPayload, EventMeta.New("", ID.Id()));
        initEvt.Version = 0;
        // AND
        var startPayload = Contract.Start.Payload.New;
        var startEvt = Event.New(ID, Start.Topics.EvtTopic, startPayload, EventMeta.New("", ID.Id()));
        startEvt.Version = 1;
        // AND
        var throttleUpPayload = Contract.ChangeRpm.Payload.New(5);
        var throttleUpEvt = Event.New(ID, ChangeRpm.Topics.EvtTopic, throttleUpPayload, EventMeta.New("", ID.Id()));
        throttleUpEvt.Version = 2;
//        var throttleUpCmd = ThrottleUp.Cmd.New(_engineID, throttleUpPayload);
        // WHEN
        return new[] { initEvt, startEvt, throttleUpEvt };
    }

    public static IEnumerable<ICmd> InitializeScenario(IID ID)
    {
        var initializePayload = Payload.New(Details.New("New Engine"));
        var initializeCmd = Cmd.New(ID, initializePayload);
        return new[] { initializeCmd };
    }

    public static async Task<IEnumerable<IFeedback>> RunScenario(IAggregate aggregate,
        IEnumerable<ICmd> commands)
    {
        var accu = new List<IFeedback>();
        foreach (var command in commands)
        {
            var fbk = await aggregate.ExecuteAsync(command);
            accu.Add(fbk);
        }

        return accu;
    }
}