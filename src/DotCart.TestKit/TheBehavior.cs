using DotCart.Abstractions.Behavior;
using DotCart.Core;

namespace DotCart.TestKit;

public static class TheBehavior
{
    [Name(TheConstants.AggregateName)]
    public interface IAggregateInfo : IAggregateInfoB
    {
    }
}