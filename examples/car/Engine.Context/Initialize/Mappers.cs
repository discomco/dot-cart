using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using Engine.Contract;

namespace Engine.Context.Initialize;

public static class Mappers
{
    public static readonly Evt2Fact<Contract.Initialize.Fact, IEvt> _evt2Fact =
        evt => Contract.Initialize.Fact.New(evt.AggregateID.Id(), evt.GetPayload<Contract.Initialize.Payload>());

    public static readonly Hope2Cmd<Cmd, Contract.Initialize.Hope> _hope2Cmd =
        hope => Cmd.New(hope.AggId.IDFromIdString(), hope.Payload);

    public static readonly Evt2State<Common.Schema.Engine, IEvt> _evt2Doc = (state, evt) =>
    {
        if (evt == null) return state;
        if (evt.GetPayload<Common.Schema.Engine>() == null) return state;
        state = evt.GetPayload<Common.Schema.Engine>();
        state.Id = evt.AggregateID.Id();
        state.Status = Schema.EngineStatus.Initialized;
        return state;
    };
}