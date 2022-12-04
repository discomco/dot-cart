using DotCart.Abstractions.Schema;

namespace Engine.TestUtils;

public static class Schema
{
    public static readonly IDCtorT<
            Contract.Schema.EngineID>
        IDCtor =
            _ => Contract.Schema.EngineID.New("E63E3CAD-0CAE-4F77-B3CE-808FB087032D");
}