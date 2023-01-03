using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Behavior;

namespace Engine.TestUtils;

public static class Stop
{
    public static readonly PayloadCtorT<Contract.Stop.Payload>
        PayloadCtor =
            Contract.Stop.Payload.New;

    public static readonly CmdCtorT<
            Contract.Schema.EngineID,
            Contract.Stop.Payload,
            EventMeta>
        CmdCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return CmdT<Contract.Stop.Payload, EventMeta>.New(
                    ID,
                    PayloadCtor(),
                    EventMeta.New(NameAtt.Get<IEngineAggregateInfo>(), ID.Id())
                );
            };


    public static readonly HopeCtorT<Contract.Stop.Payload>
        HopeCtor =
            (_, _) => HopeT<Contract.Stop.Payload>.New(
                Schema.DocIDCtor().Id(),
                PayloadCtor());

    public static readonly FactCtorT<Contract.Stop.Payload, EventMeta>
        FactCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return FactT<Contract.Stop.Payload, EventMeta>.New(
                    ID.Id(),
                    PayloadCtor(),
                    Schema.MetaCtor(ID.Id()));
            };

    public static readonly EvtCtorT<Contract.Stop.Payload, EventMeta>
        EvtCtor =
            (_, _, _) => Behavior.Stop._newEvt(
                Schema.DocIDCtor(),
                PayloadCtor(),
                Schema.MetaCtor(Schema.DocIDCtor().Id())
            );

    public static readonly Contract.Schema.EngineListItem
        StartedEngineListItem =
            Contract.Schema.EngineListItem.New(
                Schema.DocIDCtor().Id(),
                "A Started Engine",
                Contract.Schema.EngineStatus.Started,
                0);


    public static readonly StateCtorT<Contract.Schema.EngineList>
        StartedListCtor =
            () =>
            {
                var lst = Contract.Schema.EngineList.New();
                lst.Items = lst.Items.Add(
                    StartedEngineListItem.EngineId,
                    StartedEngineListItem);
                return lst;
            };
}