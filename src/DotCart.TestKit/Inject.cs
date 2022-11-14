using DotCart.Drivers.Serilog;
using DotCart.TestKit.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DotCart.TestKit;

public static partial class Inject
{
    public static IServiceCollection AddBaseTestEnv(this IServiceCollection services)
    {
        return services
            .AddHostExecutor()
            .AddConsoleLogger()
            .AddTransient<ITestOutputHelper, TestOutputHelper>()
            .AddSingleton<ITestHelper, TestHelper>();
    }
}