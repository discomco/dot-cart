using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Behavior;

namespace Engine.TestUtils;

public static class Schema
{
    public static readonly IDCtorT<Contract.Schema.EngineID>
        DocIDCtor =
            _ => Contract.Schema.EngineID.New("E63E3CAD-0CAE-4F77-B3CE-808FB087032D");

    public static readonly IDCtorT<Contract.Schema.EngineListID>
        ListIDCtor =
            _ => Contract.Schema.EngineListID.New();

    public static readonly MetaCtorT<MetaB>
        MetaCtor =
            _ => MetaB.New(NameAtt.Get<IEngineAggregateInfo>(), DocIDCtor().Id());

    public static readonly Contract.Schema.EngineListItem
        UnknownEngineListItem = 
            Contract.Schema.EngineListItem.New(
                DocIDCtor().Id(),
                "A Fine Engine",
                Contract.Schema.EngineStatus.Unknown,
                0);

    public static readonly StateCtorT<Contract.Schema.EngineList>
        FilledListCtor =
            () =>
            {
                var lst = EmptyListCtor();
                lst.Items = lst.Items.Add(
                    UnknownEngineListItem.Id,
                    UnknownEngineListItem);
                return lst;
            };

    public static readonly StateCtorT<Contract.Schema.EngineList>
        EmptyListCtor =
            () => Contract.Schema.EngineList.New(ListIDCtor().Id());


    public static readonly ValueObjectCtorT<Contract.Schema.Details>
        OldDetailsCtor =
            () => Contract.Schema.Details.New("engine #309", "Some Old Engine");

    public static readonly ValueObjectCtorT<Contract.Schema.Rpm>
        OldRpmCtor =
            () => Contract.Schema.Rpm.New(0);

    public static readonly StateCtorT<Contract.Schema.Engine>
        DocCtor =
            () => Contract.Schema.Engine.New(DocIDCtor().Id(),
                Contract.Schema.EngineStatus.Initialized,
                OldDetailsCtor(),
                OldRpmCtor());

    public static readonly ValueObjectCtorT<Contract.Schema.Details>
        DocDetailsCtor =
            () => Contract.Schema.Details.New("John Lennon", "John Lennon of the Beatles");

    public static readonly ValueObjectCtorT<Contract.Schema.Rpm>
        DocRpmCtor =
            () => Contract.Schema.Rpm.New(42);
}