using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using Engine.Contract;

namespace Engine.Utils;

public static class Initialize
{
    
    public static readonly PayloadCtorT<Contract.Initialize.Payload> TestPayloadCtor = 
        () => Contract.Initialize.Payload.New(Contract.Schema.Details.New("New Engine", "Enter Description Here"));
    
    public static readonly CmdCtorT<Behavior.Initialize.Cmd, Contract.Schema.EngineID, Contract.Initialize.Payload> TestCmdCtor =
        (_, payload) => Behavior.Initialize.Cmd.New(payload); 

    
    
    public static readonly GenerateHope<Contract.Initialize.Hope> _genHope =
        () =>
        {
            var details = Contract.Schema.Details.New("NewEngine");
            var pl = Contract.Initialize.Payload.New(details);
            return Contract.Initialize.Hope.New(pl);
        };
}