using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace Engine.TestUtils;

public static class Start
{
    public static readonly PayloadCtorT<Contract.Start.Payload>
        PayloadCtor =
            () => Contract.Start.Payload.New;

    public static readonly CmdCtorT<
            Behavior.Start.Cmd,
            Contract.Schema.EngineID,
            Contract.Start.Payload>
        CmdCtor =
            (_, _) => Behavior.Start.Cmd.New(Schema.IDCtor(), PayloadCtor());

    public static readonly HopeCtorT<
            Contract.Start.Hope, 
            Contract.Start.Payload>
        HopeCtor =
            (_, _) => Contract.Start.Hope.New(Schema.IDCtor().Id(), PayloadCtor());

    public static readonly FactCtorT<
            Contract.Start.Fact, 
            Contract.Start.Payload>
        FactCtor = 
            (_, _) => Contract.Start.Fact.New(Schema.IDCtor().Id(), PayloadCtor());

    public static readonly EvtCtorT<
            Behavior.Start.Evt, 
            Contract.Schema.EngineID>
        EvtCtor = 
            _ => Behavior.Start.Evt.New(Schema.IDCtor(), PayloadCtor());
}