using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.TestKit.Schema;

namespace DotCart.TestKit.Behavior;

public class TheAggregate : AggregateT<TheDoc>
{
    public TheAggregate(IExchange exchange, StateCtorT<TheDoc> newState) : base(exchange, newState)
    {
    }
}