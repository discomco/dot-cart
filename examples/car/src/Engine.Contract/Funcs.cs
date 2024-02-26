using DotCart.Abstractions.Schema;

namespace Engine.Contract;

public static class Funcs
{
    public static readonly ValueObjectCtorT<Schema.EngineListItem>
        EngineListItemCtor =
            () => new Schema.EngineListItem();

    public static readonly ValueObjectCtorT<Schema.Details>
        DetailsCtor =
            () => new Schema.Details();

    public static readonly IDCtorT<Schema.EngineID>
        RootIDCtor =
            _ => new Schema.EngineID();

    public static readonly IDCtorT<Schema.EngineListID>
        ListIDCtor =
            _ => new Schema.EngineListID();

    public static readonly StateCtorT<Schema.Engine>
        RootCtor =
            Schema.Engine.New;

    public static readonly StateCtorT<Schema.EngineList>
        ListCtor =
            () => Schema.EngineList.New(ListIDCtor().Id());

}