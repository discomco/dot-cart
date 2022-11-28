using DotCart.Abstractions.Schema;

namespace Engine.Utils;

public static class Schema
{
    public static readonly IDCtorT<Contract.Schema.EngineID> TestIDCtor = 
        () => new Contract.Schema.EngineID("E63E3CAD-0CAE-4F77-B3CE-808FB087032D".ToLower());
}