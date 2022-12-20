using DotCart.TestFirst.Schema;
using DotCart.TestKit;
using DotCart.TestKit.Schema;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Tests.Schema;

public class IDTests : IDTestsT<TheID>
{
    public IDTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddTransient(_ => Utils.IDCtor);
    }
}