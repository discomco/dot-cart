using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit.Abstractions;

namespace DotCart.Drivers.Microsoft.Tests;

public class Something : ISomething
{
}

public interface ISomething
{
}

public class DITests : IoCTests
{
    public DITests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveSomething()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var st = TestEnv.ResolveRequired<ISomething>();
        // THEN
        Assert.NotNull(st);
    }

    protected override void Initialize()
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services.TryAddSingleton<ISomething, Something>();
    }
}