using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Behavior;

namespace Engine.TestUtils;

public static class Schema
{
    public static readonly IDCtorT<
            Contract.Schema.EngineID>
        DocIDCtor =
            _ => Contract.Schema.EngineID.New("E63E3CAD-0CAE-4F77-B3CE-808FB087032D");

    public static readonly IDCtorT<
            Contract.Schema.EngineListID>
        ListIDCtor =
            value => Contract.Schema.EngineListID.New();

    public static readonly MetaCtorT<EventMeta>
        MetaCtor =
            _ => EventMeta.New(NameAtt.Get<IEngineAggregateInfo>(), DocIDCtor().Id());

    public static readonly Contract.Schema.EngineListItem DefaultEngineListItem =
        Contract.Schema.EngineListItem.New(
            DocIDCtor().Id(), 
            "A Fine Engine", 
            Contract.Schema.EngineStatus.Unknown, 
            0);

    public static readonly StateCtorT<Contract.Schema.EngineList>
        FilledListCtor =
            () =>
            {
                var lst = Contract.Schema.EngineList.New();
                lst.Items = lst.Items.Add(
                    DefaultEngineListItem.EngineId, 
                    DefaultEngineListItem);
                return lst;
            };
    
    public static readonly StateCtorT<Contract.Schema.EngineList>
        EmptyListCtor =
            Contract.Schema.EngineList.New;

    public static readonly ValueObjectCtorT<Contract.Schema.Details> 
        OldDetailsCtor = () => Contract.Schema.Details.New("engine #309", "Some Old Engine");

    public static readonly StateCtorT<Contract.Schema.Engine>
        DocCtor = () =>
            Contract.Schema.Engine.New(DocIDCtor().Id(), Contract.Schema.EngineStatus.Initialized, OldDetailsCtor());

}