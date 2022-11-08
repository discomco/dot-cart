using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestEnv.Engine;
public delegate IEnumerable<ICmd> ScenarioGenerator<in TID, in TState>(TID ID, NewState<TState> newState)
    where TID : IID
    where TState : IState;

public delegate IEnumerable<IEvt> EventStreamGenerator<in TID, in TState>(TID ID, NewState<TState> newState)
    where TID : IID
    where TState : IState;


public static partial class Inject
{
    public static IServiceCollection AddInitializeEngineWithThrottleUpStream(this IServiceCollection services)
    {
        return services
            .AddTransient<EventStreamGenerator<EngineID,Schema.Engine>>(_ => ScenariosAndStreams.InitializeEngineWithThrottleUpEventStream);
    }

    public static IServiceCollection AddInitializeEngineScenario(this IServiceCollection services)
    {
        return services
            .AddTransient<ScenarioGenerator<EngineID, Schema.Engine>>(_ => ScenariosAndStreams.InitializeScenario);
    }
    
}

public static class ScenariosAndStreams
{
    public static IEnumerable<IEvt> InitializeEngineWithThrottleUpEventStream(EngineID ID, NewState<Schema.Engine> newEngine)
    {
        var initPayload = Initialize.Payload.New(newEngine());
        var initEvt = Event.New(ID, Initialize.EvtTopic, initPayload, EventMeta.New("", ID.Id()));
        initEvt.Version = 0;
        // AND
        var startPayload = Start.Payload.New;
        var startEvt = Event.New(ID, Start.EvtTopic, startPayload, EventMeta.New("", ID.Id()));
        startEvt.Version = 1;
        // AND
        var throttleUpPayload = ThrottleUp.Payload.New(5);
        var throttleUpEvt = Event.New(ID, ThrottleUp.EvtTopic, throttleUpPayload, EventMeta.New("", ID.Id()));
        throttleUpEvt.Version = 2;
//        var throttleUpCmd = ThrottleUp.Cmd.New(_engineID, throttleUpPayload);
        // WHEN
        return new[] { initEvt, startEvt, throttleUpEvt };
    }

    public static IEnumerable<ICmd> InitializeScenario(IID id, NewState<Schema.Engine> newEngine)
    {
        var engine = newEngine();
        var initializePayload = Initialize.Payload.New(engine);
        var initializeCmd = Initialize.Cmd.New(id, initializePayload);
        return new[] { initializeCmd };
    }

    public static async Task<IEnumerable<IFeedback>> RunScenario(IAggregate aggregate, IEnumerable<ICmd> commands)
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