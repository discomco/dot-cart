using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Schema;

public abstract class ListDocTestsT<TListID, TList, TItem> : DocTestsT<TListID, TList>
    where TListID : IID
    where TList : IListState
    where TItem : IValueObject
{
    protected ListDocTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }
}