using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.Core;
using DotCart.TestKit.Schema;

namespace DotCart.TestKit.Behavior;

public class TheAggregate : AggregateT<ITheAggregateInfo, TheDoc>
{
    public TheAggregate(IExchange exchange, StateCtorT<TheDoc> newState)
        : base(exchange, newState)
    {
    }
}

[Name("the:aggregate")]
public interface ITheAggregateInfo : IAggregateInfoB
{
}