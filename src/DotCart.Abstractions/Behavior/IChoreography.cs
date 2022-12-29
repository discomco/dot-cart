using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public interface IChoreography
{
    void SetAggregate(IAggregate aggregate);
    string Name { get; }
    string Topic { get; }
    Task<Feedback> WhenAsync(IEvtB evt);
}