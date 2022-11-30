using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace Engine.TestUtils;

public static class Stop
{
    public static readonly PayloadCtorT<Contract.Stop.Payload>
        TestPayloadCtor =
            Contract.Stop.Payload.New;

    public static readonly CmdCtorT<
            Behavior.Stop.Cmd,
            Contract.Schema.EngineID,
            Contract.Stop.Payload>
        TestCmdCtor =
            (_, _) => Behavior.Stop.Cmd.New(Schema.TestIDCtor(), TestPayloadCtor());
}