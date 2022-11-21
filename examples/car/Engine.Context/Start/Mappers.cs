using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using Engine.Contract;

namespace Engine.Context.Start;

public static class Mappers
{
    internal static readonly Evt2Fact<Contract.Start.Fact, IEvt> _evt2Fact =
        evt => Contract.Start.Fact.New(evt.AggregateID.Id(), evt.GetPayload<Contract.Start.Payload>());

    internal static readonly Evt2State<Common.Schema.Engine, IEvt> _evt2Doc =
        (state, _) =>
        {
            ((int)state.Status).SetFlag((int)Schema.EngineStatus.Started);
            return state;
        };

    internal static readonly Hope2Cmd<Cmd, Contract.Start.Hope> _hope2Cmd =
        hope =>
            Cmd.New(hope.AggId.IDFromIdString(), hope.GetPayload<Contract.Start.Payload>());

    internal static readonly Evt2Cmd<Cmd, Initialize.IEvt> _evt2Cmd =
        evt =>
            Cmd.New(evt.AggregateID, Contract.Start.Payload.New);
}