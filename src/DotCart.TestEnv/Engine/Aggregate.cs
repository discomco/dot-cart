using DotCart.Behavior;
using DotCart.Schema;


namespace DotCart.TestEnv.Engine;

public class Aggregate : Aggregate<Schema.Engine>
{
    public Aggregate(NewState<Schema.Engine> newState) : base(newState)
    {
    }
}

