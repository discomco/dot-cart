using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.TestUtils;

public static class Start
{
    public static readonly PayloadCtorT<Contract.Start.Payload>
        PayloadCtor =
            () => Contract.Start.Payload.New;

    public static readonly CmdCtorT<
            Contract.Schema.EngineID,
            Contract.Start.Payload,
            Meta>
        CmdCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return Command.New<Contract.Start.Payload>(
                    ID,
                    PayloadCtor().ToBytes(),
                    Schema.MetaCtor(ID.Id()).ToBytes());
            };

    public static readonly HopeCtorT<Contract.Start.Payload>
        HopeCtor =
            (_, _) => HopeT<Contract.Start.Payload>.New(Schema.DocIDCtor().Id(), PayloadCtor());

    public static readonly FactCtorT<Contract.Start.Payload, Meta>
        FactCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return FactT<Contract.Start.Payload, Meta>.New(
                    ID.Id(),
                    PayloadCtor(),
                    Schema.MetaCtor(ID.Id()));
            };

    public static readonly EvtCtorT<Contract.Start.Payload, Meta>
        EvtCtor =
            (_, _, _) => Behavior.Start._newEvt(
                Schema.DocIDCtor(),
                PayloadCtor().ToBytes(),
                Schema.MetaCtor(null).ToBytes()
            );

    public static readonly StateCtorT<Contract.Schema.Engine>
        DocCtor =
            () => Contract.Schema.Engine.New(
                Schema.DocIDCtor().Id(),
                Contract.Schema.EngineStatus.Initialized,
                Contract.Schema.Details.New("Engine #32", "An Initialized Engine"));
}