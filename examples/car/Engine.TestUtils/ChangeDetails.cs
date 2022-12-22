using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Behavior;

namespace Engine.TestUtils;

public static class ChangeDetails
{
    public static readonly PayloadCtorT<
            Contract.ChangeDetails.Payload>
        PayloadCtor =
            () => Contract.ChangeDetails.Payload.New(Contract.Schema.Details.New("Engine #2", "A V8 Merlin engine."));

    public static readonly HopeCtorT<
            Contract.ChangeDetails.Hope,
            Contract.ChangeDetails.Payload>
        HopeCtor =
            (_, _) => Contract.ChangeDetails.Hope.New(Schema.IDCtor().Id(), PayloadCtor());

    public static readonly FactCtorT<
            Contract.ChangeDetails.Fact,
            Contract.ChangeDetails.Payload>
        FactCtor =
            (_, _) => Contract.ChangeDetails.Fact.New(Schema.IDCtor().Id(), PayloadCtor());

    public static readonly CmdCtorT<Behavior.ChangeDetails.Cmd,
            Contract.Schema.EngineID,
            Contract.ChangeDetails.Payload>
        CmdCtor =
            (_, _) => Behavior.ChangeDetails.Cmd.New(
                Schema.IDCtor(),
                PayloadCtor(),
                EventMeta.New(
                    NameAtt.Get<IEngineAggregateInfo>(),
                    Schema.IDCtor().Id()
                )
            );

    public static readonly EvtCtorT<Behavior.ChangeDetails.IEvt, Contract.ChangeDetails.Payload, EventMeta>
        EvtCtor =
            (_, _, _) => Behavior.ChangeDetails._newEvt(Schema.IDCtor(), PayloadCtor(), Schema.MetaCtor(null));


    [Tag(StateTags.Invalid)] public static readonly StateCtorT<Contract.Schema.Engine>
        InvalidEngineCtor =
            () => Contract.Schema.Engine.New(
                Schema.IDCtor().Id(),
                Contract.Schema.EngineStatus.Unknown,
                Contract.Schema.Details.New("Invalid Test Engine",
                    $"This is an INVALID Engine for ChangeDetails because state is [{Contract.Schema.EngineStatus.Unknown}]"));

    [Tag(StateTags.Valid)] public static readonly StateCtorT<Contract.Schema.Engine>
        ValidEngineCtor =
            () => Contract.Schema.Engine.New(
                Schema.IDCtor().Id(),
                Contract.Schema.EngineStatus.Initialized,
                Contract.Schema.Details.New("Valid Test Engine",
                    $"This is an VALID Engine for ChangeDetails because state is [{Contract.Schema.EngineStatus.Initialized}] "));
}