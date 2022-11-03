using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst;

public abstract class BehaviorTests: IoCTests
{
    protected BehaviorTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }
}