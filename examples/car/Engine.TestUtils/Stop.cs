using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace Engine.TestUtils;

public static class Stop
{
    public static readonly PayloadCtorT<Contract.Stop.Payload>
        PayloadCtor =
            Contract.Stop.Payload.New;

    public static readonly CmdCtorT<
            Behavior.Stop.Cmd,
            Contract.Schema.EngineID,
            Contract.Stop.Payload>
        CmdCtor =
            (_, _) => Behavior.Stop.Cmd.New(Schema.DocIDCtor(), PayloadCtor());

    public static readonly HopeCtorT<
            Contract.Stop.Hope,
            Contract.Stop.Payload>
        HopeCtor =
            (_, _) => Contract.Stop.Hope.New(Schema.DocIDCtor().Id(), PayloadCtor());

    public static readonly FactCtorT<
            Contract.Stop.Fact,
            Contract.Stop.Payload>
        FactCtor =
            (_, _) => Contract.Stop.Fact.New(Schema.DocIDCtor().Id(), PayloadCtor());

    public static readonly EvtCtorT<Behavior.Stop.IEvt, Contract.Stop.Payload, EventMeta>
        EvtCtor =
            (_, _, _) => Behavior.Stop._newEvt(
                Schema.DocIDCtor(),
                PayloadCtor(),
                Schema.MetaCtor(null)
            );
}