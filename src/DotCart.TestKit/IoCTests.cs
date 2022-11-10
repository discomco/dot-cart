using DotCart.TestKit.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.TestKit;

public abstract class IoCTests : OutputTests, IClassFixture<IoCTestContainer>
{
    
    protected IoCTests(ITestOutputHelper output, IoCTestContainer container) : base(output)
    {
        Container = container;
        TestHelper = Container.GetRequiredService<ITestHelper>();
        SetTestEnvironment();
        InjectDependencies(container.Services);
        Initialize();
    }


    protected IoCTestContainer Container { get; }
    protected ITestHelper TestHelper { get; }

    public override void Dispose()
    {
        Container?.Dispose();
        base.Dispose();
    }

    protected abstract void Initialize();
    protected abstract void SetTestEnvironment();
    protected abstract void InjectDependencies(IServiceCollection services);
}