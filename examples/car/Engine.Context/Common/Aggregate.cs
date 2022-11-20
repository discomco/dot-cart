using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;

namespace Engine.Context.Common;

public class Aggregate : AggregateT<Schema.Engine>
{
    public Aggregate(NewState<Schema.Engine> newState) : base(newState)
    {
    }
}