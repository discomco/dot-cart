using DotCart.Behavior;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Schema;

namespace DotCart.TestEnv.Engine.Behavior;

public interface IEnginePolicy : IDomainPolicy<IEngineAggregate>
{
}


public interface IEngineAggregate : IAggregate<Schema.Engine, EngineID>
{
   
}
public partial class EngineAggregate : Aggregate<Schema.Engine, EngineID>, IEngineAggregate
{
    public EngineAggregate(
        NewState<Schema.Engine> newState, 
        ITopicPubSub pubSub) 
        : base(newState, pubSub)
    {}
  
}