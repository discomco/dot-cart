using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using Engine.Contract.Schema;
using Engine.Contract.Start;

namespace Engine.Context.Start;





public static class Mappers
{
    internal static readonly Evt2Fact<Fact, IEvt> _evt2Fact =
        evt => Fact.New(evt.AggregateID.Id(), evt.GetPayload<Payload>());
    internal static readonly Evt2State<Common.Schema.Engine, IEvt> _evt2Doc = 
        (state, _) => {
            ((int)state.Status).SetFlag((int)EngineStatus.Started);
            return state;
        };
    internal static readonly Hope2Cmd<Cmd, Hope> _hope2Cmd =
        hope =>
            Cmd.New(hope.AggId.IDFromIdString(), hope.GetPayload<Payload>());
    internal static readonly Evt2Cmd<Cmd, Initialize.IEvt> _evt2Cmd =
        evt =>
            Cmd.New(evt.AggregateID, Payload.New);
}