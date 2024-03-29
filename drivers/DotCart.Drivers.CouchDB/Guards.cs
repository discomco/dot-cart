using Ardalis.GuardClauses;

namespace DotCart.Drivers.CouchDB;

public static class Guards
{
    public static void SmallerThanOne(this IGuardClause clause, int value, string parameterName)
    {
        if (value <= 0) throw new ArgumentException("Must not be greater than 0.", parameterName);
    }
}