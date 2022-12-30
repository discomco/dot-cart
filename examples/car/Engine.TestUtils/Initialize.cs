using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace Engine.TestUtils;

public static class Initialize
{
    public static readonly EvtCtorT<Behavior.Initialize.IEvt, Contract.Initialize.Payload, EventMeta>
        EvtCtor =
            (_, _, _) =>
                Behavior.Initialize._newEvt(
                    Schema.DocIDCtor(),
                    PayloadCtor(),
                    Schema.MetaCtor(null)
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
                Schema.DocIDCtor(),
                PayloadCtor()
            );

    public static readonly HopeCtorT<
            Contract.Initialize.Hope,
            Contract.Initialize.Payload>
        HopeCtor =
            (_, _) => Contract.Initialize.Hope.New(PayloadCtor());

    public static readonly FactCtorT<Contract.Initialize.Payload>
        FactCtor =
            (_, _) => FactT<Contract.Initialize.Payload>.New(
                Schema.DocIDCtor().Id(), 
                PayloadCtor());
}