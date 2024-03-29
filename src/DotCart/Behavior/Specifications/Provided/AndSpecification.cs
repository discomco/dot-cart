﻿namespace DotCart.Behavior.Specifications.Provided;

public class AndSpeficication<T> : Specification<T>
{
    private readonly ISpecification<T> _specification1;
    private readonly ISpecification<T> _specification2;

    public AndSpeficication(
        ISpecification<T> specification1,
        ISpecification<T> specification2)
    {
        _specification1 = specification1 ?? throw new ArgumentNullException(nameof(specification1));
        _specification2 = specification2 ?? throw new ArgumentNullException(nameof(specification2));
    }

    protected override IEnumerable<string> IsNotSatisfiedBecause(T aggregate)
    {
        return _specification1.WhyIsNotSatisfiedBy(aggregate)
            .Concat(_specification2.WhyIsNotSatisfiedBy(aggregate));
    }
}