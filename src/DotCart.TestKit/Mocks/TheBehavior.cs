using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;

namespace DotCart.TestKit.Mocks;

public static class TheBehavior
{
    [Name(TheConstants.AggregateName)]
    public interface IAggregateInfo : IAggregateInfoB;
}