using System.Collections.Concurrent;
using System.Reflection;

namespace DotCart;

public abstract record ValueObject
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyCollection<PropertyInfo>> TypeProperties =
        new();

    public override string ToString()
    {
        return $"{{{string.Join(", ", GetProperties().Select(f => $"{f.Name}: {f.GetValue(this)}"))}}}";
    }

    protected virtual IEnumerable<object> GetEqualityComponents()
    {
        return GetProperties().Select(x => x.GetValue(this));
    }

    protected virtual IEnumerable<PropertyInfo> GetProperties()
    {
        return TypeProperties.GetOrAdd(
            GetType(),
            t => t
                .GetTypeInfo()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .OrderBy(p => p.Name)
                .ToList());
    }
}