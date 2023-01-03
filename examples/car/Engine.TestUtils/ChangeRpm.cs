using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Behavior;

namespace Engine.TestUtils;

public static class ChangeRpm
{
    public static readonly CmdCtorT<Contract.Schema.EngineID, Contract.ChangeRpm.Payload, EventMeta>
        CmdCtor =
            (id, _, _) =>
                CmdT<Contract.ChangeRpm.Payload, EventMeta>.New(
                    id,
                    PayloadCtor(),
                    EventMeta.New(
                        NameAtt.Get<IEngineAggregateInfo>(),
                        id.Id()
                    ));

    public static readonly PayloadCtorT<Contract.ChangeRpm.Payload>
        PayloadCtor =
            () => Contract.ChangeRpm.Payload.New(Random.Shared.Next(-10, +10));

    public static readonly HopeCtorT<Contract.ChangeRpm.Payload>
        HopeCtor =
            (_, _) => HopeT<Contract.ChangeRpm.Payload>.New(Schema.DocIDCtor().Id(), PayloadCtor());

    public static readonly FactCtorT<Contract.ChangeRpm.Payload, EventMeta>
        FactCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return FactT<Contract.ChangeRpm.Payload, EventMeta>.New(
                    ID.Id(),
                    PayloadCtor(),
                    Schema.MetaCtor(ID.Id()));
            };

    public static readonly EvtCtorT<Contract.ChangeRpm.Payload, EventMeta>
        EvtCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return Behavior.ChangeRpm._newEvt(
                    ID,
                    PayloadCtor(),
                    Schema.MetaCtor(ID.Id())
                );
            };
}