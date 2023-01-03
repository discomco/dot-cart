using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace Engine.TestUtils;

public static class Start
{
    public static readonly PayloadCtorT<Contract.Start.Payload>
        PayloadCtor =
            () => Contract.Start.Payload.New;

    public static readonly CmdCtorT<
            Contract.Schema.EngineID,
            Contract.Start.Payload,
            EventMeta>
        CmdCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return CmdT<Contract.Start.Payload, EventMeta>.New(
                    ID,
                    PayloadCtor(),
                    Schema.MetaCtor(ID.Id()));
            };

    public static readonly HopeCtorT<Contract.Start.Payload>
        HopeCtor =
            (_, _) => HopeT<Contract.Start.Payload>.New(Schema.DocIDCtor().Id(), PayloadCtor());

    public static readonly FactCtorT<Contract.Start.Payload, EventMeta>
        FactCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return FactT<Contract.Start.Payload, EventMeta>.New(
                    ID.Id(),
                    PayloadCtor(),
                    Schema.MetaCtor(ID.Id()));
            };

    public static readonly EvtCtorT<Contract.Start.Payload, EventMeta>
        EvtCtor =
            (_, _, _) => Behavior.Start._newEvt(
                Schema.DocIDCtor(),
                PayloadCtor(),
                Schema.MetaCtor(null));

    public static readonly StateCtorT<Contract.Schema.Engine>
        DocCtor =
            () => Contract.Schema.Engine.New(
                Schema.DocIDCtor().Id(),
                Contract.Schema.EngineStatus.Initialized,
                Contract.Schema.Details.New("Engine #32", "An Initialized Engine"));
}