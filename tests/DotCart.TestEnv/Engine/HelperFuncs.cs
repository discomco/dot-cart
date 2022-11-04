using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Schema;

namespace DotCart.TestEnv.Engine;

public static class HelperFuncs
{
    public static IEnumerable<IEvt> CreateEngineEvents(IID id, NewState<Schema.Engine> newEngine)
    {
        var initPayload = Initialize.Payload.New(newEngine());
        var initEvt = Initialize.Evt.New(id, initPayload);
        // AND
        var startPayload = Start.Payload.New;
        var startEvt = Start.Evt.New(id, startPayload);
        // AND
        var throttleUpPayload = ThrottleUp.Payload.New(5);
        var throttleUpEvt = ThrottleUp.Evt.New(id, throttleUpPayload);
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