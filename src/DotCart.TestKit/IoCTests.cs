using DotCart.TestKit.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.TestKit;

public abstract class IoCTests : OutputTests, IClassFixture<IoCTestContainer>
{
    protected IoCTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output)
    {
        TestEnv = testEnv;
        TestHelper = TestEnv.ResolveRequired<ITestHelper>();
        SetEnVars();
        testEnv.Services
            .AddBaseTestEnv();
        InjectDependencies(testEnv.Services);
        Initialize();
    }


    protected IoCTestContainer TestEnv { get; }
    protected ITestHelper TestHelper { get; }

    public override void Dispose()
    {
        TestEnv.Dispose();
        base.Dispose();
    }

    protected abstract void Initialize();
    protected abstract void SetEnVars();
    protected abstract void InjectDependencies(IServiceCollection services);
}