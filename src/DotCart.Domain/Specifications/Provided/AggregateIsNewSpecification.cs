namespace DotCart.Domain.Specifications.Provided;

public class AggregateIsNewSpecification : Specification<IAggregate>
{
    protected override IEnumerable<string> IsNotSatisfiedBecause(IAggregate aggregate)
    {
        if (!aggregate.IsNew) yield return $"'{aggregate.Name}' with ID '{aggregate.Id()}' is not new";
    }
}