using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace Engine.TestUtils;

public static class Initialize
{
    public static readonly PayloadCtorT<
            Contract.Initialize.Payload>
        PayloadCtor =
            () => Contract.Initialize.Payload.New(
                Contract.Schema.Details.New(
                    "Test Engine #1",
                    "Description for Test Engine #1: This is a 1600cc Petrol Engine"));

    public static readonly CmdCtorT<
            Behavior.Initialize.Cmd, 
            Contract.Schema.EngineID, 
            Contract.Initialize.Payload>
        CmdCtor =
            (_, _) => Behavior.Initialize.Cmd.New(PayloadCtor());

    public static readonly HopeCtorT<
            Contract.Initialize.Hope, 
            Contract.Initialize.Payload>
        HopeCtor =
            (_, _) => Contract.Initialize.Hope.New(PayloadCtor());

}