using DockTrace.TestKit;
using DotCart.Drivers.Serilog;
using DotCart.TestKit.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DotCart.TestKit;

public static class Inject
{
    public static IServiceCollection AddBaseTestEnv(this IServiceCollection services)
    {
        services.TryAddSingleton<ITestHelper, TestHelper>();
        return services
            .AddHostExecutor()
            .AddConsoleLogger()
            .AddTransient<ITestOutputHelper, TestOutputHelper>();
    }
}