using DotCart.Core;

namespace DotCart.Behavior.Specifications.Provided;

public class NotSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _specification;

    public NotSpecification(
        ISpecification<T> specification)
    {
        _specification = specification ?? throw new ArgumentNullException(nameof(specification));
    }

    protected override IEnumerable<string> IsNotSatisfiedBecause(T aggregate)
    {
        if (_specification.IsSatisfiedBy(aggregate))
            yield return $"Specification '{_specification.GetType().PrettyPrint()}' should not be satisfied";
    }
}