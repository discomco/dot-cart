using DotCart.Context.Behaviors;
using Engine.Contract.Start;

namespace Engine.Context.Start;

public static class Mappers
{
    public static Evt2Cmd<Initialize.IEvt, Cmd> evt2Cmd =>
        evt =>
            Cmd.New(evt.AggregateID, Payload.New);
}