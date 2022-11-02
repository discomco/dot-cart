using DotCart.Behavior;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Schema;

namespace DotCart.TestEnv.Engine;




public partial class Aggregate : Aggregate<Schema.Engine, EngineID>
{
    public Aggregate(
        NewState<Schema.Engine> newState)
        : base(newState)
    {
    }
}