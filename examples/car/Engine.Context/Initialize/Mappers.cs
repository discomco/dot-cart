using DotCart.Context.Effects;
using DotCart.Contract.Schemas;
using Engine.Contract.Initialize;
using Engine.Contract.Schema;

namespace Engine.Context.Initialize;

public static class Mappers
{
    public static readonly Evt2Fact<Fact, IEvt> _evt2Fact =
        evt => Fact.New(evt.AggregateID.Id(), evt.GetPayload<Payload>());

    public static readonly Hope2Cmd<Cmd, Hope> _hope2Cmd =
        hope => Cmd.New(hope.AggId.IDFromIdString(), hope.GetPayload<Payload>());

    public static Evt2State<Common.Schema.Engine, IEvt> _evt2State => (state, evt) =>
    {
        if (evt == null) return state;
        if (evt.GetPayload<Common.Schema.Engine>() == null) return state;
        state = evt.GetPayload<Common.Schema.Engine>();
        state.Id = evt.AggregateID.Id();
        state.Status = EngineStatus.Initialized;
        return state;
    };
}