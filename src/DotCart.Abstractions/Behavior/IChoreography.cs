using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public interface IChoreography
{
    string Name { get; }
    string Topic { get; }
    void SetAggregate(IAggregate aggregate);
    Task<Feedback> WhenAsync(IEvtB evt);
}