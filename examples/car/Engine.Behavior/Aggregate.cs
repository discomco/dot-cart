using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;

namespace Engine.Behavior;

public class Aggregate : AggregateT<Engine>
{
    public Aggregate(StateCtorT<Engine> newState) : base(newState)
    {
    }
}