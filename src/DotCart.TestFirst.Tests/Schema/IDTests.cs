using DotCart.Abstractions.Schema;
using DotCart.TestFirst.Schema;
using DotCart.TestKit;
using DotCart.TestKit.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Tests.Schema;

[IDPrefix(TheConstants.IDPrefix)]
public class IDTests : IDTestsT<TheSchema.ID>
{
    public IDTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddTransient(_ => Utils.IDCtor);
    }
}