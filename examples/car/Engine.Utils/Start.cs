using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using Engine.Contract;

namespace Engine.Utils;

public static class Start
{
    public static readonly PayloadCtorT<Contract.Start.Payload> 
        TestPayloadCtor =
        () => Contract.Start.Payload.New;

    public static readonly CmdCtorT<
        Behavior.Start.Cmd, 
        Contract.Schema.EngineID, 
        Contract.Start.Payload> 
        TestCmdCtor =
        (_, _) => Behavior.Start.Cmd.New(Schema.TestIDCtor(), TestPayloadCtor());

    
}