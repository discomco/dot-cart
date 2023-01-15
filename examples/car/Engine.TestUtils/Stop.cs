using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.TestUtils;

public static class Stop
{
    public static readonly PayloadCtorT<Contract.Stop.Payload>
        PayloadCtor =
            Contract.Stop.Payload.New;

    public static readonly CmdCtorT<
            Contract.Schema.EngineID,
            Contract.Stop.Payload,
            MetaB>
        CmdCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return Command.New<Contract.Stop.Payload>(
                    ID,
                    PayloadCtor().ToBytes(),
                    MetaB.New(NameAtt.Get<IEngineAggregateInfo>(), ID.Id()).ToBytes()
                );
            };


    public static readonly HopeCtorT<Contract.Stop.Payload>
        HopeCtor =
            (_, _) => HopeT<Contract.Stop.Payload>.New(
                Schema.DocIDCtor().Id(),
                PayloadCtor());

    public static readonly FactCtorT<Contract.Stop.Payload, MetaB>
        FactCtor =
            (_, _, _) =>
            {
                var ID = Schema.DocIDCtor();
                return FactT<Contract.Stop.Payload, MetaB>.New(
                    ID.Id(),
                    PayloadCtor(),
                    Schema.MetaCtor(ID.Id()));
            };

    public static readonly EvtCtorT<Contract.Stop.Payload, MetaB>
        EvtCtor =
            (_, _, _) => Behavior.Stop._newEvt(
                Schema.DocIDCtor(),
                PayloadCtor().ToBytes(),
                Schema.MetaCtor(Schema.DocIDCtor().Id()).ToBytes()
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
                    StartedEngineListItem.Id,
                    StartedEngineListItem);
                return lst;
            };

    public static IServiceCollection AddTestFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Schema.DocIDCtor)
            .AddTransient(_ => Schema.MetaCtor)
            .AddTransient(_ => PayloadCtor)
            .AddTransient(_ => FactCtor);
    }
}