using DotCart.Context.Abstractions;
using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Contract.Schemas;
using Engine.Contract.Schema;
using Engine.Contract.Start;

namespace Engine.Context.Start;

public static class Mappers
{
    internal static readonly Evt2Fact<Fact, IEvt> _evt2Fact =
        evt => Fact.New(evt.AggregateID.Id(), evt.GetPayload<Payload>());

    internal static readonly Evt2State<Common.Schema.Engine, IEvt> _evt2State = (state, _) =>
    {
        ((int)state.Status).SetFlag((int)EngineStatus.Started);
        return state;
    };

    internal static readonly Hope2Cmd<Cmd, Hope> _hope2Cmd =
        hope =>
            Cmd.New(hope.AggId.IDFromIdString(), hope.GetPayload<Payload>());


    public static Evt2Cmd<Initialize.IEvt, Cmd> evt2Cmd =>
        evt =>
            Cmd.New(evt.AggregateID, Payload.New);
}