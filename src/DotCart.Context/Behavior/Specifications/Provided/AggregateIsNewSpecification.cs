using DotCart.Abstractions.Behavior;

namespace DotCart.Context.Behavior.Specifications.Provided;

public class AggregateIsNewSpecification : Specification<IAggregate>
{
    protected override IEnumerable<string> IsNotSatisfiedBecause(IAggregate aggregate)
    {
        if (!aggregate.IsNew) yield return $"'{aggregate.GetName()}' with ID '{aggregate.Id()}' is not new";
    }
}