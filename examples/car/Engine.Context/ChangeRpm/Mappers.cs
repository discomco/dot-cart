using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;

namespace Engine.Context.ChangeRpm;

public static class Mappers
{
    public static readonly Evt2State<Common.Schema.Engine, IEvt> _evt2State = (state, evt) =>
    {
        state.Power += evt.GetPayload<Contract.ChangeRpm.Payload>().Delta;
        return state;
    };

    public static Evt2Fact<Contract.ChangeRpm.Fact, IEvt> _evt2Fact =>
        evt => Contract.ChangeRpm.Fact.New(
            evt.AggregateID.Id(),
            evt.GetPayload<Contract.ChangeRpm.Payload>());

    public static Hope2Cmd<Cmd, Contract.ChangeRpm.Hope> _hope2Cmd =>
        hope => Cmd.New(hope.AggId.IDFromIdString(), hope.GetPayload<Contract.ChangeRpm.Payload>());
}