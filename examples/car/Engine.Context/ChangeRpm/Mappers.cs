using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using Engine.Contract.ChangeRpm;

namespace Engine.Context.ChangeRpm;

public static class Mappers
{
    public static readonly Evt2State<Common.Schema.Engine, IEvt> _evt2State = (state, evt) =>
    {
        state.Power += evt.GetPayload<Payload>().Delta;
        return state;
    };

    public static Evt2Fact<Fact, IEvt> _evt2Fact =>
        evt => Fact.New(
            evt.AggregateID.Id(),
            evt.GetPayload<Payload>());

    public static Hope2Cmd<Cmd, Hope> _hope2Cmd =>
        hope => Cmd.New(hope.AggId.IDFromIdString(), hope.GetPayload<Payload>());
}