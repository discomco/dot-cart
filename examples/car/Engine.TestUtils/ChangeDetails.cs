using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.TestUtils;

public static class ChangeDetails
{
    public static PayloadCtorT<
            Contract.ChangeDetails.Payload>
        PayloadCtor =
            () => Contract.ChangeDetails.Payload.New(Contract.Schema.Details.New("Engine #2", "A V8 Merlin engine."));

    public static HopeCtorT<
            Contract.ChangeDetails.Hope,
            Contract.ChangeDetails.Payload>
        HopeCtor =
            (_, _) => Contract.ChangeDetails.Hope.New(Schema.IDCtor().Id(), PayloadCtor());

    public static FactCtorT<
            Contract.ChangeDetails.Fact,
            Contract.ChangeDetails.Payload>
        FactCtor =
            (_, _) => Contract.ChangeDetails.Fact.New(Schema.IDCtor().Id(), PayloadCtor());

    public static CmdCtorT<Behavior.ChangeDetails.Cmd,
            Contract.Schema.EngineID,
            Contract.ChangeDetails.Payload>
        CmdCtor =
            (_, _) => Behavior.ChangeDetails.Cmd.New(Schema.IDCtor(), PayloadCtor());

    public static EvtCtorT<Behavior.ChangeDetails.Evt, Contract.Schema.EngineID>
        EvtCtor = 
            _ => Behavior.ChangeDetails.Evt.New(Schema.IDCtor(), PayloadCtor()); 

}