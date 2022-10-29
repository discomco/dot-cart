using Ardalis.GuardClauses;

namespace DotCart.Drivers.Ardalis;

public class DotGuard : IClause
{
    public static IClause Against => (IClause)Guard.Against;
}

public interface IClause : IGuardClause
{
}