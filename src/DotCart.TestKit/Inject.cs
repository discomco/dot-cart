using DotCart.TestKit.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestKit;

public static partial class Inject
{
    public static IServiceCollection AddTestHelpers(this IServiceCollection services)
    {
        return services
            .AddHostExecutor()
            .AddSingleton<ITestHelper, TestHelper>();
    }
}