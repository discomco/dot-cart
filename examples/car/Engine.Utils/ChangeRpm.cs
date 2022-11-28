using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using Engine.Contract;

namespace Engine.Utils;

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


    public static GenerateHope<Contract.ChangeRpm.Hope> _generateHope => () =>
    {
        var engineID = Schema.TestIDCtor();
        var pl = Contract.ChangeRpm.Payload.New(Random.Shared.Next(20));
        return Contract.ChangeRpm.Hope.New(engineID.Id(), pl);
    };
}