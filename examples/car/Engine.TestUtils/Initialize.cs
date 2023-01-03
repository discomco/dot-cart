using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.TestUtils;

public static class Initialize
{
    public static readonly EvtCtorT<Contract.Initialize.Payload, EventMeta>
        EvtCtor =
            (_, _, _) =>
                Behavior.Initialize._newEvt(
                    Schema.DocIDCtor(),
                    PayloadCtor().ToBytes(),
                    Schema.MetaCtor(null).ToBytes()
                );


    public static readonly PayloadCtorT<
            Contract.Initialize.Payload>
        PayloadCtor =
            () => Contract.Initialize.Payload.New(
                Contract.Schema.Details.New(
                    "Test Engine #1",
                    "Description for Test Engine #1: This is a 1600cc Petrol Engine"));

    public static readonly CmdCtorT<
            Contract.Schema.EngineID,
            Contract.Initialize.Payload,
            EventMeta>
        CmdCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return Command.New<Contract.Initialize.Payload>(
                    ID,
                    PayloadCtor().ToBytes(),
                    Schema.MetaCtor(ID.Id()).ToBytes()
                );
            };


    public static readonly HopeCtorT<Contract.Initialize.Payload>
        HopeCtor =
            (_, _) => HopeT<Contract.Initialize.Payload>.New(
                Schema.DocIDCtor().Id(),
                PayloadCtor()
            );

    public static readonly FactCtorT<Contract.Initialize.Payload, EventMeta>
        FactCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return FactT<Contract.Initialize.Payload, EventMeta>.New(
                    ID.Id(),
                    PayloadCtor(),
                    Schema.MetaCtor(ID.Id()));
            };
}