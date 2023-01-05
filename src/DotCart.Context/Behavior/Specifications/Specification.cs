namespace DotCart.Context.Behavior.Specifications;

public interface ISpecification<in T>
{
    bool IsSatisfiedBy(T obj);

    IEnumerable<string> WhyIsNotSatisfiedBy(T obj);
}

public abstract class Specification<T> : ISpecification<T>
{
    public bool IsSatisfiedBy(T obj)
    {
        return !IsNotSatisfiedBecause(obj).Any();
    }

    public IEnumerable<string> WhyIsNotSatisfiedBy(T obj)
    {
        return IsNotSatisfiedBecause(obj);
    }

    protected abstract IEnumerable<string> IsNotSatisfiedBecause(T aggregate);
}