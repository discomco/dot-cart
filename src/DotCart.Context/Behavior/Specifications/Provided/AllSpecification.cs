namespace DotCart.Context.Behavior.Specifications.Provided;

public class AllSpecifications<T> : Specification<T>
{
    private readonly IReadOnlyList<ISpecification<T>> _specifications;

    public AllSpecifications(
        IEnumerable<ISpecification<T>> specifications)
    {
        if (!specifications.Any())
            throw new ArgumentException("Please provide some specifications", nameof(specifications));
        _specifications = specifications.ToList();
    }

    protected override IEnumerable<string> IsNotSatisfiedBecause(T aggregate)
    {
        return _specifications.SelectMany(s => s.WhyIsNotSatisfiedBy(aggregate));
    }
}