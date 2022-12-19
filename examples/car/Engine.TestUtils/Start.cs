using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Behavior;

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
            Event,
            Contract.Schema.EngineID>
        EvtCtor =
            _ => Behavior.Start.NewEvt(
                Schema.IDCtor(),
                PayloadCtor(),
                EventMeta.New(
                    NameAtt.Get<IEngineAggregateInfo>(),
                    Schema.IDCtor().Id()
                )
            );
}