using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.Drivers.EventStoreDB.TestFirst;

public abstract class ProjectionTestsT(
    ITestOutputHelper output,
    IoCTestContainer testEnv)
    : IoCTests(output, testEnv);