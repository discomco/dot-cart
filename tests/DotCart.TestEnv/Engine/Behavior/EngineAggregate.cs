using DotCart.Behavior;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Schema;

namespace DotCart.TestEnv.Engine.Behavior;

public partial class EngineAggregate : Aggregate<Schema.Engine, EngineID>
{
    public EngineAggregate(
        NewState<Schema.Engine> newState,
        ITopicPubSub pubSub)
        : base(newState, pubSub)
    {
    }
}