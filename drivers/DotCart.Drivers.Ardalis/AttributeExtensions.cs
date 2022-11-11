using Ardalis.GuardClauses;

namespace DotCart.Drivers.Ardalis;

public static class AttributeExtensions
{
    public static void AttributeNotDefined(this IGuardClause clause, string attributeName, Attribute[] attributes,
        string className)
    {
        Guard.Against.Null(attributes, nameof(attributes));
        if (attributes.Length == 0)
            throw new Exception($"Attribute [{attributeName}] is not defined on type {className}");
    }
}