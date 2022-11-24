using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.TestKit.Schema;

namespace DotCart.TestKit.Behavior;

public class TheAggregate : AggregateT<TheDoc>
{
    public TheAggregate(NewState<TheDoc> newState) : base(newState)
    {
    }
}