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
            (_, _) => Behavior.Start.Cmd.New(Schema.DocIDCtor(), PayloadCtor());

    public static readonly HopeCtorT<
            Contract.Start.Hope,
            Contract.Start.Payload>
        HopeCtor =
            (_, _) => Contract.Start.Hope.New(Schema.DocIDCtor().Id(), PayloadCtor());

    public static readonly FactCtorT<
            Contract.Start.Fact,
            Contract.Start.Payload>
        FactCtor =
            (_, _) => Contract.Start.Fact.New(Schema.DocIDCtor().Id(), PayloadCtor());

    public static readonly EvtCtorT<Behavior.Start.IEvt, Contract.Start.Payload, EventMeta>
        EvtCtor =
            (_, _, _) => Behavior.Start._newEvt(Schema.DocIDCtor(), PayloadCtor(), Schema.MetaCtor(null));

    public static readonly StateCtorT<Contract.Schema.Engine>
        DocCtor =
            () => Contract.Schema.Engine.New(
                Schema.DocIDCtor().Id(),
                Contract.Schema.EngineStatus.Initialized,
                Contract.Schema.Details.New("Engine #32", "An Initialized Engine"));
}