using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace Engine.TestUtils;

public static class ChangeRpm
{
    public static readonly CmdCtorT<
        Behavior.ChangeRpm.Cmd,
        Contract.Schema.EngineID,
        Contract.ChangeRpm.Payload> RandomCmdCtor =
        (id, _) => Behavior.ChangeRpm.Cmd.New(id, RandomPayloadCtor());

    public static readonly PayloadCtorT<
        Contract.ChangeRpm.Payload> RandomPayloadCtor =
        () => Contract.ChangeRpm.Payload.New(Random.Shared.Next(-10, +10));
}