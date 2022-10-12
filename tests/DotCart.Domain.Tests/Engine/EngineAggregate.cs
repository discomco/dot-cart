using DotCart.Schema;
using DotCart.Schema.Tests;

namespace DotCart.Domain.Tests.Engine;

public interface IEnginePolicy : IAggregatePolicy<IEngineAggregate>
{
}


public interface IEngineAggregate : IAggregate<Schema.Tests.Engine, EngineID>
{
   
}
public partial class EngineAggregate : Aggregate<Schema.Tests.Engine, EngineID>, IEngineAggregate
{
    public EngineAggregate(
        NewState<Schema.Tests.Engine> newState, 
        ITopicPubSub pubSub) 
        : base(newState, pubSub)
    {}
  
}