using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Behavior;

namespace Engine.TestUtils;

public static class ChangeRpm
{
    public static readonly CmdCtorT<
            Behavior.ChangeRpm.Cmd,
            Contract.Schema.EngineID,
            Contract.ChangeRpm.Payload>
        CmdCtor =
            (id, _) => Behavior.ChangeRpm.Cmd.New(id, PayloadCtor(), EventMeta.New(
                NameAtt.Get<IEngineAggregateInfo>(),
                id.Id()
            ));

    public static readonly PayloadCtorT<
            Contract.ChangeRpm.Payload>
        PayloadCtor =
            () => Contract.ChangeRpm.Payload.New(Random.Shared.Next(-10, +10));

    public static readonly HopeCtorT<
            Contract.ChangeRpm.Hope,
            Contract.ChangeRpm.Payload>
        HopeCtor =
            (_, _) => Contract.ChangeRpm.Hope.New(Schema.DocIDCtor().Id(), PayloadCtor());

    public static readonly FactCtorT<
            Contract.ChangeRpm.Fact,
            Contract.ChangeRpm.Payload>
        FactCtor =
            (_, _) => Contract.ChangeRpm.Fact.New(Schema.DocIDCtor().Id(), PayloadCtor());

    public static readonly EvtCtorT<Behavior.ChangeRpm.IEvt, Contract.ChangeRpm.Payload, EventMeta>
        EvtCtor =
            (_, _, _) => Behavior.ChangeRpm._newEvt(
                Schema.DocIDCtor(),
                PayloadCtor(),
                Schema.MetaCtor(null));
}