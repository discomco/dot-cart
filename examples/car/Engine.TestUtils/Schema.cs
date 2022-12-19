using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Behavior;

namespace Engine.TestUtils;

public static class Schema
{
    public static readonly IDCtorT<
            Contract.Schema.EngineID>
        IDCtor =
            _ => Contract.Schema.EngineID.New("E63E3CAD-0CAE-4F77-B3CE-808FB087032D");
    
    public static readonly MetaCtor 
        MetaCtor = 
            _ => EventMeta.New(NameAtt.Get<IEngineAggregateInfo>(), IDCtor().Id()); 
}