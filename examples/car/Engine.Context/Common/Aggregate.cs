using DotCart.Context.Behaviors;
using DotCart.Contract.Schemas;

namespace Engine.Context.Common;

public class Aggregate : Aggregate<Schema.Engine>
{
    public Aggregate(NewState<Schema.Engine> newState) : base(newState)
    {
    }
}