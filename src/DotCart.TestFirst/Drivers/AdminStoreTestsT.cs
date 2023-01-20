using DotCart.Abstractions.Drivers;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class AdminStoreTestsT<TDbInfo>
    : StoreTestsT<TDbInfo>
    where TDbInfo : IDbInfoB
{
    protected AdminStoreTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }
}