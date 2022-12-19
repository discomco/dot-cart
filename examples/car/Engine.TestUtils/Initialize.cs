using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Behavior;

namespace Engine.TestUtils;

public static class Initialize
{
    public static readonly EvtCtorT<Event, Contract.Schema.EngineID>
        EvtCtor =
            _ => Behavior.Initialize.NewEvt(
                Schema.IDCtor(),
                PayloadCtor(),
                EventMeta.New(
                    NameAtt.Get<IEngineAggregateInfo>(),
                    Schema.IDCtor().Id()
                )
            );

    public static readonly PayloadCtorT<
            Contract.Initialize.Payload>
        PayloadCtor =
            () => Contract.Initialize.Payload.New(
                Contract.Schema.Details.New(
                    "Test Engine #1",
                    "Description for Test Engine #1: This is a 1600cc Petrol Engine"));

    public static readonly CmdCtorT<
            Behavior.Initialize.Cmd,
            Contract.Schema.EngineID,
            Contract.Initialize.Payload>
        CmdCtor =
            (_, _) => Behavior.Initialize.Cmd.New(
                Schema.IDCtor(),
                PayloadCtor()
            );

    public static readonly HopeCtorT<
            Contract.Initialize.Hope,
            Contract.Initialize.Payload>
        HopeCtor =
            (_, _) => Contract.Initialize.Hope.New(PayloadCtor());

    public static readonly FactCtorT<
            Contract.Initialize.Fact,
            Contract.Initialize.Payload>
        FactCtor =
            (_, _) => Contract.Initialize.Fact.New(Schema.IDCtor().Id(), PayloadCtor());
}