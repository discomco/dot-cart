using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.Core;
using DotCart.TestKit.Schema;

namespace DotCart.TestKit.Behavior;

public class TheAggregate : AggregateT<ITheAggregateInfo, TheDoc>
{
    public TheAggregate(StateCtorT<TheDoc> newState)
        : base(newState)
    {
    }
}

[Name("the:aggregate")]
public interface ITheAggregateInfo : IAggregateInfoB
{
}