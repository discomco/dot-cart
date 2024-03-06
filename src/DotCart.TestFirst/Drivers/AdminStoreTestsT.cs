using DotCart.Abstractions.Drivers;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class AdminStoreTestsT<TDbInfo>(
    ITestOutputHelper output,
    IoCTestContainer testEnv)
    : StoreTestsT<TDbInfo>(output, testEnv)
    where TDbInfo : IDbInfoB;