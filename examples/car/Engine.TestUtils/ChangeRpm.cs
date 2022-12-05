using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace Engine.TestUtils;

public static class ChangeRpm
{
    public static readonly CmdCtorT<
            Behavior.ChangeRpm.Cmd,
            Contract.Schema.EngineID,
            Contract.ChangeRpm.Payload>
        CmdCtor =
            (id, _) => Behavior.ChangeRpm.Cmd.New(id, PayloadCtor());

    public static readonly PayloadCtorT<
            Contract.ChangeRpm.Payload>
        PayloadCtor =
            () => Contract.ChangeRpm.Payload.New(Random.Shared.Next(-10, +10));

    public static readonly HopeCtorT<
            Contract.ChangeRpm.Hope,
            Contract.ChangeRpm.Payload>
        HopeCtor =
            (_, _) => Contract.ChangeRpm.Hope.New(Schema.IDCtor().Id(), PayloadCtor());

    public static readonly FactCtorT<
            Contract.ChangeRpm.Fact,
            Contract.ChangeRpm.Payload>
        FactCtor =
            (_, _) => Contract.ChangeRpm.Fact.New(Schema.IDCtor().Id(), PayloadCtor());
    
    public static readonly EvtCtorT<
        Behavior.ChangeRpm.Evt, 
        Contract.Schema.EngineID>
        EvtCtor = 
            _ => Behavior.ChangeRpm.Evt.New(Schema.IDCtor(), PayloadCtor());



}