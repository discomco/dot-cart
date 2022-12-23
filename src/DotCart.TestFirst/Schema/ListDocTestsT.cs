using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Schema;

public abstract class ListDocTestsT<TListID, TList, TItemID, TItem> : DocTestsT<TListID, TList>
    where TListID : IID
    where TList : IListState
    where TItemID : IID
    where TItem : IEntityT<TItemID>
{
    protected ListDocTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }
    
    
    
}